using Capacities.Active_Capacities.Grab;
using Entities;
using Entities.Capacities;
using Entities.Champion;
using GameStates;
using Photon.Pun;
using UnityEngine;

namespace Capacities.Active_Capacities
{
    public class GrabCapacity : ActiveCapacity
    {
        public override bool TryCast(int casterIndex, int[] targetsEntityIndexes, Vector3[] targetPositions)
        {
            // Todo - fix cooldown, on crée une nouvelle capacité à chaque utilisation donc ça override le cooldown !

            // Check condition
            if (onCooldown)
            {
                Debug.LogWarning("You're on a cooldown for grab capacity!");
                return false;
            }

            // Cast
            var caster = EntityCollectionManager.GetEntityByIndex(casterIndex);

            if (EntityCollectionManager.GetEntityByIndex(targetsEntityIndexes[0]) == null)
            {
                Debug.Log("You targeted no entity. Grabbing forward!");
                var forward = ((Champion)caster).rotateParent.forward;
                Debug.DrawRay(caster.transform.position, forward * 5, Color.red, 5f);
                var so = (GrabCapacitySO)AssociatedActiveCapacitySO();
                var go = so.grabHookPrefab;

                var cast = PhotonNetwork.Instantiate(go.name, ((Champion)caster).rotateParent.position + forward,
                    Quaternion.LookRotation(forward)).GetComponent<GrabHook>();

                cast.caster = caster;
                cast.Shoot();
            }

            InitiateCooldown();
            return true;
        }

        public override void PlayFeedback(int casterIndex, int[] targetsEntityIndexes, Vector3[] targetPositions)
        {
            Debug.Log("Feedback!");
        }
    }
}