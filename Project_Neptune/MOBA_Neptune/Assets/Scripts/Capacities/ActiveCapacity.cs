using GameStates;
using UnityEngine;

namespace Entities.Capacities
{
    public abstract class ActiveCapacity
    {
        public byte indexOfSOInCollection;
        public Entity caster;
        private double cooldownTimer;
        public bool onCooldown;
        private double feedbackTimer;
        
        public GameObject instantiateFeedbackObj;

        protected int target;

        protected ActiveCapacitySO AssociatedActiveCapacitySO()
        {
            return CapacitySOCollectionManager.GetActiveCapacitySOByIndex(indexOfSOInCollection);
        }

        #region Cast
        
        /// <summary>
        /// Check if the target is in range.
        /// </summary>
        /// <returns></returns>
        private bool IsTargetInRange()
        {
            Debug.Log(caster);
            Debug.Log(target);
            //get the distance between the entity and the target
            float distance = Vector3.Distance(caster.transform.position, EntityCollectionManager.GetEntityByIndex(target).transform.position);
            //if the distance is lower than the range, return true
            if (distance < AssociatedActiveCapacitySO().maxRange)
            {
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Initialize the cooldown of the capacity when used.
        /// </summary>
        protected virtual void InitiateCooldown()
        {
        
            cooldownTimer = AssociatedActiveCapacitySO().cooldown;
            onCooldown = true;
         
            
            GameStateMachine.Instance.OnTick += CooldownTimer;
        }

        /// <summary>
        /// Method which update the timer.
        /// </summary>
        protected virtual void CooldownTimer()
        {
            cooldownTimer -= 1.0 / GameStateMachine.Instance.tickRate;
            
            if (cooldownTimer <= 0)
            {
                Debug.Log("Cooldown is over");
                onCooldown = false;
                Debug.Log(onCooldown);
                GameStateMachine.Instance.OnTick -= CooldownTimer;
            }
        }
        
        /// <summary>
        /// Called when trying cast a capacity.
        /// </summary>
        /// <param name="casterIndex"></param>
        /// <param name="targetsEntityIndexes"></param>
        /// <param name="targetPositions"></param>
        /// <returns></returns>
        public virtual bool TryCast(int casterIndex, int[] targetsEntityIndexes, Vector3[] targetPositions)
        {
            // if (Vector3.Distance(EntityCollectionManager.GetEntityByIndex(casterIndex).transform.position, EntityCollectionManager.GetEntityByIndex(targetsEntityIndexes[0]).transform.position)> 
            //     AssociatedActiveCapacitySO().maxRange) return false;
            
            if (!onCooldown)
            {
                InitiateCooldown();
                return true;
            }
            else return false;
        }

        public virtual bool isInRange(int casterIndex, Vector3 position)
        {
            float distance = Vector3.Distance(EntityCollectionManager.GetEntityByIndex(casterIndex).transform.position, position);
            //Debug.Log($"distance:{distance}  >  range:{ AssociatedActiveCapacitySO().maxRange}");
            if ( distance > AssociatedActiveCapacitySO().maxRange) return false;
            
            return true;
        }

        #endregion

        #region Feedback

        public abstract void PlayFeedback(int casterIndex, int[] targetsEntityIndexes, Vector3[] targetPositions);

        protected virtual void InitializeFeedbackCountdown()
        {
            feedbackTimer = AssociatedActiveCapacitySO().feedbackDuration;
            GameStateMachine.Instance.OnTick += FeedbackCountdown;
        }

        protected virtual void FeedbackCountdown()
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
            GameStateMachine.Instance.OnTick -= FeedbackCountdown;
        }
        
        #endregion
        
    }
}

