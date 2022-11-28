using Entities.FogOfWar;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UnityEngine.Rendering.Universal
{
    public class FogPass : ScriptableRenderPass
    {
        public FilterMode filterMode { get; set; }
        public FogFeature.Settings settings;

        RenderTargetIdentifier _source;
        RenderTargetIdentifier destination;
        int temporaryRTId = Shader.PropertyToID("_TempRT");

        int sourceId;
        int destinationId;

        string m_ProfilerTag;


        //fog
        FogOfWarManager mFog;
        Camera mCam;
        Matrix4x4 mInverseMVP;

        public FogPass(string tag)
        {
            m_ProfilerTag = tag;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor blitTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            blitTargetDescriptor.depthBufferBits = 0;

            var renderer = renderingData.cameraData.renderer;

            sourceId = -1;
            _source = renderer.cameraColorTarget;

            destinationId = temporaryRTId;
            cmd.GetTemporaryRT(destinationId, blitTargetDescriptor, filterMode);
            destination = new RenderTargetIdentifier(destinationId);

            //setup Fog Of War
            if (mFog == null)
            {
                mFog = FogOfWarManager.Instance;
            }

            mCam = Camera.main;
        }

        /// <inheritdoc/>
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!Application.isPlaying)return;
            if (mFog != null)
            {
                if(!mFog.sceneToRenderFog.Contains(SceneManager.GetActiveScene().name)) return;
            }
            else
            {
                if (!settings.sceneToRenderFog.Contains(SceneManager.GetActiveScene().name)) return;
            }
            //fog
            SendShaderValue();
            
            CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);
            
            Blit(cmd, _source, destination, settings.blitMaterial, settings.blitMaterialPassIndex);
            Blit(cmd, destination, _source);
            
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }   

        /// <inheritdoc/>
        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (destinationId != -1)
                cmd.ReleaseTemporaryRT(destinationId);

            if (_source == destination && sourceId != -1)
                cmd.ReleaseTemporaryRT(sourceId);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            mFog.fogColor = Color.black;
        }

        void SendShaderValue()
        {
            if (mFog == null)
            {
                return;
            }

            float blendFactor = 1;

            // Calculate the inverse modelview-projection matrix to convert screen coordinates to world coordinates
            mInverseMVP = (mCam.projectionMatrix * mCam.worldToCameraMatrix).inverse;

            float invScale = 1f / mFog.worldSize;
            Transform t = mFog.transform;
            float x = t.position.x - mFog.worldSize * 0.5f;
            float y = t.position.z - mFog.worldSize * 0.5f;

            Vector4 camPos = mCam.transform.position;

            //Check the quality settings of the camera.
            if (QualitySettings.antiAliasing > 0)
            {
                RuntimePlatform pl = Application.platform;

                if (pl == RuntimePlatform.WindowsEditor ||
                    pl == RuntimePlatform.WindowsPlayer ||
                    pl == RuntimePlatform.WebGLPlayer)
                {
                    camPos.w = 1f;
                }
            }

            
            Vector4 p = new Vector4(-x * invScale, -y * invScale, invScale, blendFactor);
            //Send the data back to the material
            settings.blitMaterial.SetColor("_Unexplored", mFog.fogColor);
           // Debug.Log(mFog.fogColor);
            settings.blitMaterial.SetMatrix("_InverseMVP", mInverseMVP);
            settings.blitMaterial.SetVector("_CamPos", camPos);
            settings.blitMaterial.SetVector("_Params", p);
        }
    }
}
