using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;

namespace Entities.Capacities
{
    public class PassiveSlowEffect : PassiveCapacity
    {
        private PassiveSlowEffectSO so;
        private IMoveable moveable;
        public override PassiveCapacitySO AssociatedPassiveCapacitySO()
        {
            return CapacitySOCollectionManager.Instance.GetPassiveCapacitySOByIndex(indexOfSo);
        }

        protected override void OnAddedEffects(Entity target)
        {
            so = (PassiveSlowEffectSO)AssociatedPassiveCapacitySO();
            moveable = target.GetComponent<IMoveable>();
            moveable?.DecreaseCurrentMoveSpeedRPC(so.slowAmount);
        }

        protected override void OnAddedFeedbackEffects(Entity target)
        {
            if(moveable == null) entity.passiveCapacitiesList.Remove(this);
        }

        protected override void OnRemovedEffects(Entity target)
        {
            moveable.IncreaseCurrentMoveSpeedRPC(so.slowAmount);
        }

        protected override void OnRemovedFeedbackEffects(Entity target)
        {
            
        }
    }
}

