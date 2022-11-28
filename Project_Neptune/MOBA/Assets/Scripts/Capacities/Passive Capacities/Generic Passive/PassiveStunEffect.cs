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
            return CapacitySOCollectionManager.Instance.GetPassiveCapacitySOByName(so.name);
        }

        public override void OnAdded(Entity target)
        {
            base.OnAdded(target);

            so = (PassiveStunEffectSO)AssociatedPassiveCapacitySO();
            moveable = target.GetComponent<IMoveable>();

            moveable.SetCanMoveRPC(false);
            GameStateMachine.Instance.OnTick += WaitBeforeRelease;
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


