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
            return CapacitySOCollectionManager.Instance.GetPassiveCapacitySOByIndex(indexOfSo);
        }

        protected override void OnAddedEffects(Entity target)
        {
            so = (PassiveDotEffectSO)AssociatedPassiveCapacitySO();
            
            lifeable = entity.GetComponent<IActiveLifeable>();
            
            GameStateMachine.Instance.OnTick += WaitForTickDamage;
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
    }
}


