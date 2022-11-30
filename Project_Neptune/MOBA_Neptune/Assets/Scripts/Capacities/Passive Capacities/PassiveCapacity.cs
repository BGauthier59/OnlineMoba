using System;
using System.Collections.Generic;
using Entities;
using Entities.Capacities;

namespace Capacities.Passive_Capacities
{
    public abstract class PassiveCapacity
    {
        public byte indexOfSo; //Index Reference in CapacitySOCollectionManager
        public bool stackable;
        private int count; //Amount of Stacks

        public List<Enums.CapacityType> types; //All types of the capacity
        
        protected Entity targetEntity;

        public PassiveCapacitySO AssociatedPassiveCapacitySO()
        {
            return CapacitySOCollectionManager.GetPassiveCapacitySOByIndex(indexOfSo);
        }
        
        public void OnAdded(Entity target)
        {
            if (stackable) count++;
            targetEntity = target;
            OnAddedEffects();
        }

        /// <summary>
        /// Call when a Stack of the capacity is Added
        /// </summary>
        protected abstract void OnAddedEffects();

        /// <summary>
        /// Call Feedback of the Stack on when Added
        /// </summary>
        public void OnAddedFeedback(Entity target)
        {
            targetEntity = target;
            OnAddedFeedbackEffects();
        }

        protected abstract void OnAddedFeedbackEffects();

        /// <summary>
        /// Call when a Stack of the capacity is Removed
        /// </summary>
        public void OnRemoved(Entity target)
        {
            OnRemovedEffects(target);
        }

        protected abstract void OnRemovedEffects(Entity target);

        /// <summary>
        /// Call Feedback of the Stack on when Removed
        /// </summary>
        public void OnRemovedFeedback(Entity target)
        {
            OnRemovedFeedbackEffects(target);
        }

        protected abstract void OnRemovedFeedbackEffects(Entity target);
    }
}