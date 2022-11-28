using System;
using Entities.Champion;
using UnityEngine;

namespace Controllers
{
    public class StreamManager : MonoBehaviour
    {
        public static StreamManager Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }

            Instance = this;
        }

        public static Vector3 GetStreamVector(Champion champion)
        {
            if (!champion.currentStreamModifier) return Vector3.zero;

            var modifier = champion.currentStreamModifier;
            if (modifier.streamModifierType == StreamModifierType.Linear)
            {
                var dir = modifier.direction switch
                {
                    StreamDirection.LeftToRight => Vector3.right,
                    StreamDirection.RightToLeft => Vector3.left,
                    _ => Vector3.zero
                };
                return dir * (modifier.streamStrength * Time.fixedDeltaTime);
            }
            else
            {
                // Todo - Debug circular : ça marche pas
                
                var relativeVector = -(modifier.circleStreamCenter.position - champion.transform.position);
                Debug.DrawRay(modifier.circleStreamCenter.position, relativeVector, Color.black);
                var dir = Vector2.Perpendicular(new Vector2(relativeVector.x, relativeVector.z));
                if (modifier.direction == StreamDirection.RightToLeft) dir = -dir;
                var dir3 = new Vector3(dir.x, 0, dir.y);

                return dir * (modifier.streamStrength * Time.fixedTime);
            }
        }
    }
}