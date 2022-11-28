using GameStates;

namespace Entities.Capacities
{
    public class PassiveStunEffect : PassiveCapacity
    {
        private PassiveStunEffectSO so;
        private IMoveable moveable;
        private double timer;
    
        public override PassiveCapacitySO AssociatedPassiveCapacitySO()
        {
            return CapacitySOCollectionManager.Instance.GetPassiveCapacitySOByIndex(indexOfSo);
        }

        protected override void OnAddedEffects(Entity target)
        {
            so = (PassiveStunEffectSO)AssociatedPassiveCapacitySO();
            moveable = target.GetComponent<IMoveable>();

            moveable.SetCanMoveRPC(false);
            GameStateMachine.Instance.OnTick += WaitBeforeRelease;
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

        private void WaitBeforeRelease()
        {
            timer += GameStateMachine.Instance.tickRate;

            if (timer >= so.stunDuration)
            {
                moveable.SetCanMoveRPC(true);
                GameStateMachine.Instance.OnTick -= WaitBeforeRelease;
            }
        }
    }
}


