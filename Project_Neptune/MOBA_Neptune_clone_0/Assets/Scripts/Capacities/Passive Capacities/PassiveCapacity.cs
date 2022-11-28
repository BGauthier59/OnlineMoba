using System.Collections.Generic;
using UnityEngine;

namespace Entities.Capacities
{
    public abstract class PassiveCapacity
    {
        public byte indexOfSo; //Index Reference in CapacitySOCollectionManager

        public bool stackable;
        private int count; //Amount of Stacks

        public List<Enums.CapacityType> types; //All types of the capacity

        public abstract PassiveCapacitySO AssociatedPassiveCapacitySO();

        protected Entity entity;
        
        
        public void OnAdded(Entity target)
        {
            if (stackable) count++;
            entity = target;
            Debug.Log("onadded");
            OnAddedEffects(entity);
        }

        /// <summary>
        /// Call when a Stack of the capicity is Added
        /// </summary>
        protected abstract void OnAddedEffects(Entity target);

        /// <summary>
        /// Call Feedback of the Stack on when Added
        /// </summary>
        public void OnAddedFeedback(Entity target)
        {
            entity = target;
            OnAddedFeedbackEffects(target);
        }
        
        protected abstract void OnAddedFeedbackEffects(Entity target);

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
