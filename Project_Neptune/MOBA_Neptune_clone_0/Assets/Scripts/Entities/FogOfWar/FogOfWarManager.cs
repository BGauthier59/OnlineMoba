using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Entities.FogOfWar
{
    public class FogOfWarManager : MonoBehaviourPun
    {
        //Instance => talk to the group to see if that possible
        private static FogOfWarManager _instance;

        public static FogOfWarManager Instance
        {
            get { return _instance; }
        }

        private void Awake()
        {
            cameraFog.GetComponent<Camera>().orthographicSize = worldSize * 0.5f;
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        /// <summary>
        /// List Of all IFogOfWarViewable for Fog of War render
        /// </summary>
        /// <param name="IFogOfWarViewable"> Interface for Entity </param>
        private List<Entity> allViewables = new List<Entity>();
        private Dictionary<Entity, List<Entity>> currentViewablesWithEntitiesShowables =
            new Dictionary<Entity, List<Entity>>();


        [Header("Camera and Scene Setup")] public Camera cameraFog;
        public List<string> sceneToRenderFog;

        [Header("Fog Of War Parameter")] [Tooltip("Color for the area where the player can't see")]
        public Color fogColor = new Color(0.25f, 0.25f, 0.25f, 1f);

        public LayerMask layerTargetFogOfWar;

        [Tooltip("Material who is going to be render in the RenderPass")]
        public Material fogMat;

        [Tooltip("Define the size of the map to make the texture fit the RenderPass")]
        public int worldSize = 24;

        public bool worldSizeGizmos;

        //Parameter For Creating Field Of View Mesh
        public FOVSettings settingsFOV;

        private void RenderFOW()
        {
            foreach (var viewable in allViewables)
            {
                if (GameStates.GameStateMachine.Instance.GetPlayerTeam() == viewable.team)
                {
                    DrawFieldOfView(viewable);
                }
            }
        }


        /// <summary>
        /// Add Entity To the Fog Of War render
        /// </summary>
        /// <param name="viewable"></param>
        public void AddFOWViewable(Entity viewable)
        {
            allViewables.Add(viewable);
            currentViewablesWithEntitiesShowables.Add(viewable, new List<Entity>());
            Debug.Log(viewable.name);
            viewable.meshFilterFoV.gameObject.SetActive(true);
        }

        /// <summary>
        /// Remove Entity To the Fog Of War render
        /// </summary>
        /// <param name="viewable"></param>
        public void RemoveFOWViewable(Entity viewable)
        {
            allViewables.Remove(viewable);
            currentViewablesWithEntitiesShowables.Remove(viewable);
            viewable.meshFilterFoV.gameObject.SetActive(false);
        }


        void SetUpCurrentViewablesWithEntitiesShowables()
        {
            foreach (var viewable in currentViewablesWithEntitiesShowables)
            {
               viewable.Value.Clear();
            }
        }

        private void Update()
        {
            SetUpCurrentViewablesWithEntitiesShowables();
            RenderFOW();
            RemoveShowablesOutOfViewables();
        }

        private void RemoveShowablesOutOfViewables()
        {
            foreach (var viewable in allViewables)
            {
                var seenShowables = viewable.seenShowables;
                for (int i = seenShowables.Count-1; i >= 0; i--)
                {
                    if (!currentViewablesWithEntitiesShowables[viewable].Contains((Entity)seenShowables[i]))
                    {
                        viewable.RemoveShowable(seenShowables[i]);
                        //Debug.Log("Remove Elements from list");
                    }
                    
                }
                
            }
        }

        public void InitMesh(MeshFilter viewMeshFilter)
        {
            Mesh viewMesh = new Mesh();
            viewMeshFilter.name = "View Mesh";
            viewMeshFilter.mesh = viewMesh;
        }

        //Draw the Field of View for the Player.
        public void DrawFieldOfView(Entity entity)
        {
            int stepCount = Mathf.RoundToInt(entity.viewAngle * settingsFOV.meshResolution / 5);
            float stepAngleSize = entity.viewAngle / stepCount;
            List<Vector3> viewPoints = new List<Vector3>();
            ViewCastInfo oldViewCast = new ViewCastInfo();

            for (int i = 0; i <= stepCount; i++)
            {
                float angle = entity.viewAngle / 2 + stepAngleSize * i;
                ViewCastInfo newViewCast = ViewCast(angle, entity);

                if (i > 0)
                {
                    bool edgeDstThresholdExceeded =
                        Mathf.Abs(oldViewCast.dst - newViewCast.dst) > settingsFOV.edgeDstThreshold;
                    if (oldViewCast.hit != newViewCast.hit ||
                        (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                    {
                        EdgeInfo edge = FindEdge(oldViewCast, newViewCast, entity);
                        if (edge.pointA != Vector3.zero)
                        {
                            viewPoints.Add(edge.pointA);
                        }

                        if (edge.pointB != Vector3.zero)
                        {
                            viewPoints.Add(edge.pointB);
                        }
                    }
                }

                viewPoints.Add(newViewCast.point);
                oldViewCast = newViewCast;
            }

            int vertexCount = viewPoints.Count + 1;
            Vector3[] vertices = new Vector3[vertexCount];
            int[] triangles = new int[(vertexCount - 2) * 3];

            vertices[0] = Vector3.zero;
            for (int i = 0; i < vertexCount - 1; i++)
            {
                vertices[i + 1] = entity.transform.InverseTransformPoint(viewPoints[i]) +
                                  Vector3.forward * settingsFOV.maskCutawayDst;

                if (i < vertexCount - 2)
                {
                    triangles[i * 3] = 0;
                    triangles[i * 3 + 1] = i + 1;
                    triangles[i * 3 + 2] = i + 2;
                }
            }

            Mesh viewMesh = entity.meshFilterFoV.mesh;
            if (viewMesh == null)
            {
                InitMesh(entity.meshFilterFoV);
            }

            viewMesh.Clear();

            viewMesh.vertices = vertices;
            viewMesh.triangles = triangles;
            viewMesh.RecalculateNormals();
        }

        EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast, Entity entity)
        {
            float minAngle = minViewCast.angle;
            float maxAngle = maxViewCast.angle;
            Vector3 minPoint = Vector3.zero;
            Vector3 maxPoint = Vector3.zero;

            for (int i = 0; i < settingsFOV.edgeResolveIterations; i++)
            {
                float angle = (minAngle + maxAngle) / 2;
                ViewCastInfo newViewCast = ViewCast(angle, entity);

                bool edgeDstThresholdExceeded =
                    Mathf.Abs(minViewCast.dst - newViewCast.dst) > settingsFOV.edgeDstThreshold;
                if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
                {
                    minAngle = angle;
                    minPoint = newViewCast.point;
                }
                else
                {
                    maxAngle = angle;
                    maxPoint = newViewCast.point;
                }
            }
            return new EdgeInfo(minPoint, maxPoint);
        }

        ViewCastInfo ViewCast(float globalAngle, Entity entity)
        {
            Vector3 dir = DirFromAngle(globalAngle, true, entity);
            RaycastHit hit;
            if (Physics.Raycast(entity.fogOfWarStartDetection.position, dir, out hit, entity.viewRange, layerTargetFogOfWar))
            {
     //           Debug.DrawRay(entity.transform.position, dir * entity.viewRange, Color.green, 1);
                Entity candidateEntity = hit.collider.gameObject.GetComponent<Entity>();
       //         Debug.Log(hit.collider.gameObject.name);
                if (candidateEntity != null)
                {
                    entity.AddShowable(candidateEntity);
                    currentViewablesWithEntitiesShowables[entity].Add(candidateEntity);
                    return new ViewCastInfo(false, entity.transform.position + dir * entity.viewRange, entity.viewRange,
                        globalAngle);
                }
                return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
            }
            else
            {
                return new ViewCastInfo(false, entity.transform.position + dir * entity.viewRange, entity.viewRange,
                    globalAngle);
            }
        }

        private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal, Entity entity)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += entity.transform.eulerAngles.y;
            }

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        public struct ViewCastInfo
        {
            public bool hit;
            public Vector3 point;
            public float dst;
            public float angle;

            public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
            {
                hit = _hit;
                point = _point;
                dst = _dst;
                angle = _angle;
            }
        }

        public struct EdgeInfo
        {
            public Vector3 pointA;
            public Vector3 pointB;

            public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
            {
                pointA = _pointA;
                pointB = _pointB;
            }
        }

        private void OnDrawGizmos()
        {
            if (!worldSizeGizmos) return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, new Vector3(worldSize * 0.9f, 10, worldSize * 0.9f));
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(worldSize, 10, worldSize));
        }
    }
}

[System.Serializable]
public class FOVSettings
{
    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDstThreshold;
    public float maskCutawayDst = .1f;
}