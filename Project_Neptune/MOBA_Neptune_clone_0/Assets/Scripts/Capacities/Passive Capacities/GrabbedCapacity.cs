using System;
using Capacities.Active_Capacities;
using Entities;
using Entities.Capacities;
using Entities.Champion;
using GameStates;
using UnityEngine;

namespace Capacities.Passive_Capacities
{
    public class GrabbedCapacity : PassiveCapacity
    {
        private double duration;
        private double timer;

        protected override void OnAddedEffects()
        {
            Debug.Log("Effect begins!");
            var grabDirection = (giverEntity.transform.position - entityUnderEffect.transform.position).normalized;
            Debug.DrawRay(entityUnderEffect.transform.position, grabDirection * 3, Color.cyan, 5);

            var soData = (GrabbedCapacitySO)AssociatedPassiveCapacitySO();

            ((Champion)entityUnderEffect).SetMoveDirection(grabDirection * soData.grabStrength);

            duration = soData.duration;
            timer = soData.timer;

            GameStateMachine.Instance.OnTick += CheckTimer;
        }

        protected override void OnAddedFeedbackEffects() { }

        protected override void OnRemovedEffects(Entity target) { }

        protected override void OnRemovedFeedbackEffects(Entity target) { }

        private void CheckTimer()
        {
            timer += 1.0 / GameStateMachine.Instance.tickRate;

            if (timer >= duration)
            {
                timer = 0;
                GameStateMachine.Instance.OnTick -= CheckTimer;
            }
        }
    }
}