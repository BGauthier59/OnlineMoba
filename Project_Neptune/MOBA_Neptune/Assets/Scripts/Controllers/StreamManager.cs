using UnityEngine;

namespace Controllers
{
    public class StreamManager : MonoBehaviour
    {
        public static StreamManager Instance;
        public float streamStrength;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }

            Instance = this;
        }

        public static Vector3 GetStreamVector(StreamModifier sM, Transform entityPos)
        {
            if (!sM) return Vector3.zero;
            
            if (sM.streamModifierType == StreamModifierType.Linear)
            {
                var dir = sM.direction switch
                {
                    StreamDirection.LeftToRight => Vector3.right,
                    StreamDirection.RightToLeft => Vector3.left,
                    _ => Vector3.zero
                };
                return dir * (sM.streamStrength * sM.linearFactor * Time.fixedDeltaTime);
            }
            else
            {
                var relativeVector = -(sM.circleStreamCenter.position - entityPos.position);
                var dir = Vector2.Perpendicular(new Vector2(relativeVector.x, relativeVector.z));
                if (sM.direction == StreamDirection.RightToLeft) dir = -dir;
                var dir3 = new Vector3(dir.x, 0, dir.y);

                return dir3 * (sM.streamStrength * Time.fixedDeltaTime);
            }
        }
    }
}