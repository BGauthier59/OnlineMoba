using Entities;
using Entities.Champion;
using Entities.Interfaces;
using GameStates;
using Photon.Pun;
using UnityEngine;

namespace Capacities.Passive_Capacities
{
    public class GrabbedCapacity : PassiveCapacity
    {
        private double duration;
        private double timer;

        private GrabbedCapacitySO data;
        private IDisplaceable displaceable;

        protected override void OnAddedEffects()
        {
            Debug.Log("Effect begins!");
            InputManager.PlayerMap.Movement.Disable();
            data = (GrabbedCapacitySO)AssociatedPassiveCapacitySO();

            var grabable = entityUnderEffect.GetComponent<IGrabable>();
            grabable?.OnGrabbed();

            duration = data.duration;
            timer = 0;

            displaceable = entityUnderEffect.GetComponent<IDisplaceable>();
            if (displaceable == null)
            {
                Debug.LogWarning("Can't displace this grabable entity?");
                return;
            }

            GameStateMachine.Instance.OnTick += MoveGrabbedEntity;
        }

        private void MoveGrabbedEntity()
        {
            var pointToReach = giverEntity != null ? giverEntity.transform.position : pos;
            pointToReach.y = 0;

            var distance = Vector3.Distance(entityUnderEffect.transform.position, pointToReach);
            
            var velocity = (pointToReach - entityUnderEffect.transform.position) * (distance * data.speed);
            velocity.y = 0;
            displaceable.SetVelocity(velocity);
            
            if (distance < 1.2f)
            {
                GameStateMachine.Instance.OnTick -= MoveGrabbedEntity;
                Debug.Log("Should have reached point!");
                GrabbedEntityHitTarget();
            }
        }

        private void GrabbedEntityHitTarget()
        {
            if (giverEntity != null) entityUnderEffect.transform.SetParent(giverEntity.transform);
            GameStateMachine.Instance.OnTick += CheckTimer;
        }

        protected override void OnAddedFeedbackEffects()
        {
            
        }
        

        protected override void OnRemovedEffects(Entity target)
        {
            Debug.Log("Not grabbed anymore");
            if (giverEntity != null) entityUnderEffect.transform.SetParent(null);
            ((Champion)entityUnderEffect).OnUnGrabbed();
            InputManager.PlayerMap.Movement.Enable();
        }

        protected override void OnRemovedFeedbackEffects(Entity target) { }

        private void CheckTimer()
        {
            timer += 1.0 / GameStateMachine.Instance.tickRate;

            if (timer >= duration - .1)
            {
                timer = 0;
                GameStateMachine.Instance.OnTick -= CheckTimer;
                entityUnderEffect.RemovePassiveCapacityByIndex(indexOfSo);
            }
        }
    }
}