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
            return CapacitySOCollectionManager.Instance.GetPassiveCapacitySOByName(so.name);
        }

        public override void OnAdded(Entity target)
        {            
            moveable = target.GetComponent<IMoveable>();

            if(moveable == null) return;
            
            base.OnAdded(target);

            moveable.DecreaseCurrentMoveSpeedRPC(so.slowAmount);
        }

        public override void OnAddedFeedback()
        {
            base.OnAddedFeedback();
        }

        public override void OnRemoved()
        {
            base.OnRemoved();
            
            moveable.IncreaseCurrentMoveSpeedRPC(so.slowAmount);
        }

        public override void OnRemoveFeedback()
        {
            base.OnRemoveFeedback();
        }
    }
}

