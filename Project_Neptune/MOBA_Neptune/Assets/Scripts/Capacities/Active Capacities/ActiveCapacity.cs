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

        #endregion
    }
}