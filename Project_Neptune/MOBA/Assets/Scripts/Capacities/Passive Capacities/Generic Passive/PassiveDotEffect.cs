using GameStates;

namespace Entities.Capacities
{
    public class PassiveDotEffect : PassiveCapacity
    {
        private PassiveDotEffectSO so;
        private IActiveLifeable lifeable;
        private double tickDamageTimer;
        private double durationTimer;
        
        public override PassiveCapacitySO AssociatedPassiveCapacitySO()
        {
            return CapacitySOCollectionManager.Instance.GetPassiveCapacitySOByName(so.name);
        }

        public override void OnAdded(Entity target)
        {
            base.OnAdded(target);

            lifeable = entity.GetComponent<IActiveLifeable>();
            
            GameStateMachine.Instance.OnTick += WaitForTickDamage;
        }

        private void WaitForTickDamage()
        {
            tickDamageTimer += GameStateMachine.Instance.tickRate;
            durationTimer += GameStateMachine.Instance.tickRate;

            if (tickDamageTimer >= so.damageTickSpeed)
            {
                lifeable.DecreaseCurrentHpRPC(so.damage);
                tickDamageTimer = 0;
            }

            if (durationTimer >= so.duration)
            {
                lifeable.DecreaseCurrentHpRPC(so.damage);
                GameStateMachine.Instance.OnTick -= WaitForTickDamage;
            }
            
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


