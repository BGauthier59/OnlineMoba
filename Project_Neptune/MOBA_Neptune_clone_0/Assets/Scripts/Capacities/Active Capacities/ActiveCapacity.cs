using GameStates;
using UnityEngine;

namespace Entities.Capacities
{
    public abstract class ActiveCapacity
    {
        public byte indexOfSOInCollection;
        public Entity caster;
        protected double cooldownTimer;
        protected bool onCooldown;
        private double feedbackTimer;
        public GameObject instantiateFeedbackObj;

        protected int target;

        protected ActiveCapacitySO AssociatedActiveCapacitySO()
        {
            return CapacitySOCollectionManager.GetActiveCapacitySOByIndex(indexOfSOInCollection);
        }

        #region Cast

        protected virtual void InitiateCooldown()
        {
            cooldownTimer = AssociatedActiveCapacitySO().cooldown;
            onCooldown = true;
            GameStateMachine.Instance.OnTick += CooldownTimer;
        }

        protected virtual void CooldownTimer()
        {
            cooldownTimer -= 1.0 / GameStateMachine.Instance.tickRate;

            if (cooldownTimer > 0) return;
            
            onCooldown = false;
            GameStateMachine.Instance.OnTick -= CooldownTimer;
        }

        public abstract bool TryCast(int casterIndex, int[] targetsEntityIndexes, Vector3[] targetPositions);

        protected Vector3 GetCasterPos()
        {
            var casterPos = caster.transform.position;
            casterPos.y = 1;
            return casterPos;
        }

        #endregion

        #region Feedback

        public abstract void PlayFeedback(int casterIndex, int[] targetsEntityIndexes, Vector3[] targetPositions);

        protected virtual void InitializeFeedbackCountdown()
        {
            feedbackTimer = AssociatedActiveCapacitySO().feedbackDuration;
            GameStateMachine.Instance.OnTick += FeedbackCooldown;
        }

        protected virtual void FeedbackCooldown()
        {
            feedbackTimer -= GameStateMachine.Instance.tickRate;

            if (feedbackTimer <= 0)
            {
                DisableFeedback();
            }
        }

        protected virtual void DisableFeedback()
        {
            PoolLocalManager.Instance.EnqueuePool(AssociatedActiveCapacitySO().feedbackPrefab, instantiateFeedbackObj);
            GameStateMachine.Instance.OnTick -= FeedbackCooldown;
        }

        #endregion
    }
}