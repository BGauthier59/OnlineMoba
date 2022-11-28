using System.Collections.Generic;

namespace Entities.Capacities
{
    public abstract class PassiveCapacity
    {
        private byte indexOfSo; //Index Reference in CapacitySOCollectionManager

        private int count; //Amount of Stacks

        public List<Enums.CapacityType> types; //All types of the capacity

        public abstract PassiveCapacitySO AssociatedPassiveCapacitySO();

        protected Entity entity;
        
        /// <summary>
        /// Call when a Stack of the capicity is Added
        /// </summary>
        public virtual void OnAdded(Entity target)
        {
            entity = target;
            entity.passiveCapacitiesList.Add(this);
        }

        /// <summary>
        /// Call Feedback of the Stack on when Added
        /// </summary>
        public virtual void OnAddedFeedback()
        {
            entity.passiveCapacitiesList.Add(this);
        }

        /// <summary>
        /// Call when a Stack of the capacity is Removed
        /// </summary>
        public virtual void OnRemoved()
        {
            entity.passiveCapacitiesList.Remove(this);
        }

        /// <summary>
        /// Call Feedback of the Stack on when Removed
        /// </summary>
        public virtual void OnRemoveFeedback()
        {
            entity.passiveCapacitiesList.Remove(this);
        }
    }
}
