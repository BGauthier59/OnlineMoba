using System.Numerics;

namespace Entities.Capacities
{
    public class PassiveKnockbackEffect : PassiveCapacity
    {
        private PassiveKnockbackEffectSO so;
        public Vector3 direction;
        
        public override PassiveCapacitySO AssociatedPassiveCapacitySO()
        {
            return CapacitySOCollectionManager.Instance.GetPassiveCapacitySOByName(so.name);
        }

        public override void OnAdded(Entity target)
        {
            base.OnAdded(target);
            
            //TODO Rigidbody PassiveKnockbackEffect
        }

        public override void OnAddedFeedback()
        {
            base.OnAddedFeedback();
        }

        public override void OnRemoved()
        {
            base.OnRemoved();
        }

        public override void OnRemoveFeedback()
        {
            base.OnRemoveFeedback();
        }
    }
}


