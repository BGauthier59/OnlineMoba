using System;
using System.Collections.Generic;
using Entities;
using Entities.Capacities;
using UnityEngine;

namespace Capacities.Passive_Capacities
{
    public abstract class PassiveCapacity
    {
        public byte indexOfSo; //Index Reference in CapacitySOCollectionManager
        public bool stackable;
        private int count; //Amount of Stacks
        public Enums.PassiveType type;

        public List<Enums.CapacityType> types; //All types of the capacity
        
        public Entity entityUnderEffect;
        public Entity giverEntity;
        public Vector3 pos;

        public PassiveCapacitySO AssociatedPassiveCapacitySO()
        {
            return CapacitySOCollectionManager.GetPassiveCapacitySOByIndex(indexOfSo);
        }
        
        public void OnAdded(Entity entityUnderEffect, Entity giver, Vector3 pos)
        {
            if (stackable) count++;
            this.entityUnderEffect = entityUnderEffect;
            giverEntity = giver;
            this.pos = pos;
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
            entityUnderEffect = target;
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