using Entities;
using Entities.Capacities;
using Entities.Champion;
using Photon.Pun;
using UnityEngine;

namespace Capacities.Active_Capacities
{
    public class BipedeAACapacity : ActiveCapacity
    {
        public override bool TryCast(int casterIndex, int[] targetsEntityIndexes, Vector3[] targetPositions)
        {
            // Todo - fix cooldown, on crée une nouvelle capacité à chaque utilisation donc ça override le cooldown !
            // Check condition
            if (onCooldown)
            {
                Debug.LogWarning("You're on a cooldown for Bipede_AA !"); return false;
            }

            // Cast de la compétence
            var caster = EntityCollectionManager.GetEntityByIndex(casterIndex);
            if (!EntityCollectionManager.GetEntityByIndex(targetsEntityIndexes[0]))
            {
                var forward = ((Champion)caster).rotateParent.forward;
                Debug.DrawRay(caster.transform.position, forward * 3, Color.magenta, 2f);
                var so = (BipedeAACapacitySO)AssociatedActiveCapacitySO();
                var prefabGo = so.projectilePrefab;

                var projectile = PhotonNetwork.Instantiate(prefabGo.name, ((Champion)caster).rotateParent.position + forward,
                    Quaternion.LookRotation(forward)).GetComponent<BipedeAAProjectile>();

                projectile.SendBipedeAA(caster);
            }
            
            InitiateCooldown();
            return true;
        }

        public override void PlayFeedback(int casterIndex, int[] targetsEntityIndexes, Vector3[] targetPositions)
        {
        }
    }
}