using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace UnityEngine.Rendering.Universal
{
    public enum BufferType
    {
        CameraColor,
        Custom
    }

    public class FogFeature : ScriptableRendererFeature
    {
        [System.Serializable]
        public class Settings
        {
            public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
            public Material blitMaterial;
            public int blitMaterialPassIndex = -1;
            public List<string> sceneToRenderFog;
        }

        public Settings settings = new Settings();
        FogPass blitPass;

        public override void Create()
        {
            blitPass = new FogPass(name);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (settings.blitMaterial == null)
            {
                Debug.LogWarningFormat("Missing Blit Material. {0} blit pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
                return;
            }

            blitPass.renderPassEvent = settings.renderPassEvent;
            blitPass.settings = settings;
            renderer.EnqueuePass(blitPass);
        }
    }
}