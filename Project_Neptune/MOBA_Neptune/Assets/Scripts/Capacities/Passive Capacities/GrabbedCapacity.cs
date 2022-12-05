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

        public bool hasHitTarget;

        protected override void OnAddedEffects()
        {
            Debug.Log("Effect begins!");
            hasHitTarget = false;
            InputManager.PlayerMap.Movement.Disable();

            var soData = (GrabbedCapacitySO)AssociatedPassiveCapacitySO();

            SetMoveDirection();

            duration = soData.duration;
            timer = 0;

            GameStateMachine.Instance.OnTick += CheckDistance;
        }

        private void SetMoveDirection()
        {
            var grabDirection = giverEntity != null
                ? (giverEntity.transform.position - entityUnderEffect.transform.position).normalized
                : (pos - entityUnderEffect.transform.position).normalized;

            var soData = (GrabbedCapacitySO)AssociatedPassiveCapacitySO();

            ((Champion)entityUnderEffect).SetMoveDirection(grabDirection * soData.grabStrength);

            Debug.DrawRay(entityUnderEffect.transform.position, grabDirection * 3, Color.cyan, 5);
        }

        public void OnEntityUnderEffectHitsTarget()
        {
            hasHitTarget = true;
            ((Champion)entityUnderEffect).OnGrabbed();
            ((Champion)entityUnderEffect).SetMoveDirection(Vector3.zero);
            Debug.Log("Hit target!");

            GameStateMachine.Instance.OnTick += CheckTimer; // Timer en restant accroché
        }

        protected override void OnAddedFeedbackEffects() { }

        protected override void OnRemovedEffects(Entity target)
        {
            Debug.Log("Not grabbed anymore");
            ((Champion)entityUnderEffect).OnUnGrabbed();
            InputManager.PlayerMap.Movement.Enable();
        }

        protected override void OnRemovedFeedbackEffects(Entity target) { }

        private void CheckDistance()
        {
            var distance = giverEntity != null
                ? Vector3.Distance(entityUnderEffect.transform.position, giverEntity.transform.position)
                : Vector3.Distance(entityUnderEffect.transform.position, pos);

            if (distance < 1.5f)
            {
                GameStateMachine.Instance.OnTick -= CheckDistance;
                Debug.Log("Should have reached point!");
                OnEntityUnderEffectHitsTarget();
            }
            else if (entityUnderEffect.rb.velocity.magnitude < .1f)
            {
                Debug.LogWarning("Has been stop while grabbing! Considers grab ended.");
                GameStateMachine.Instance.OnTick -= CheckDistance;
                Debug.Log("Should have reached point!");
                OnEntityUnderEffectHitsTarget();
            }
            else
            {
                if (giverEntity) SetMoveDirection(); // S'il y a une cible, sa position change
            }
        }

        private void CheckTimer()
        {
            timer += 1.0 / GameStateMachine.Instance.tickRate;
            Debug.Log(timer);

            if (timer >= duration - .1)
            {
                timer = 0;
                GameStateMachine.Instance.OnTick -= CheckTimer;
                entityUnderEffect.RemovePassiveCapacityByIndex(indexOfSo);
            }
        }
    }
}