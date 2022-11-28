using System.Numerics;

namespace Entities.Capacities
{
    public class PassiveKnockbackEffect : PassiveCapacity
    {
        public Vector3 direction;
        
        public override PassiveCapacitySO AssociatedPassiveCapacitySO()
        {
            return CapacitySOCollectionManager.Instance.GetPassiveCapacitySOByIndex(indexOfSo);
        }

        protected override void OnAddedEffects(Entity target)
        {
            //TODO Rigidbody PassiveKnockbackEffect
        }

        protected override void OnAddedFeedbackEffects(Entity target)
        {
            
        }

        protected override void OnRemovedEffects(Entity target)
        {
            
        }

        protected override void OnRemovedFeedbackEffects(Entity target)
        {
            
        }
    }
}


