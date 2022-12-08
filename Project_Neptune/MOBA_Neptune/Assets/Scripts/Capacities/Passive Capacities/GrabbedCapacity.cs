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
        private double securityTimer;

        private GrabbedCapacitySO data;
        private IDisplaceable displaceable;

        protected override void OnAddedEffects()
        {
            InputManager.PlayerMap.Movement.Disable();
            data = (GrabbedCapacitySO) AssociatedPassiveCapacitySO();

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
            pointToReach.y = 1;

            var distance = Vector3.Distance(entityUnderEffect.transform.position, pointToReach);

            var velocity = (pointToReach - entityUnderEffect.transform.position) * (distance * data.speed);
            velocity.y = 0;
            displaceable.SetVelocity(velocity);

            Debug.Log("Grabbed !!!");

            securityTimer += 1.0 / GameStateMachine.Instance.tickRate;

            if (securityTimer >= 5)
            {
                Debug.LogWarning($"Can't reach its target at pos {pointToReach}");
                GrabbedEntityHitTarget();

            }
            if (!(distance < 1.2f)) return;
            Debug.Log("Should have reached point!");
            GrabbedEntityHitTarget();
        }

        private void GrabbedEntityHitTarget()
        {
            if (giverEntity != null)
            {
                entityUnderEffect.transform.position = giverEntity.transform.position - entityUnderEffect.transform.forward * .5f;
                entityUnderEffect.transform.SetParent(giverEntity.transform);
            }

            GameStateMachine.Instance.OnTick -= MoveGrabbedEntity;
            GameStateMachine.Instance.OnTick += CheckTimer;
        }

        protected override void OnAddedFeedbackEffects()
        {
        }


        protected override void OnRemovedEffects(Entity target)
        {
            Debug.Log("Not grabbed anymore");
            if (giverEntity != null) entityUnderEffect.transform.SetParent(null);
            ((Champion) entityUnderEffect).OnUnGrabbed();
            InputManager.PlayerMap.Movement.Enable();
        }

        protected override void OnRemovedFeedbackEffects(Entity target)
        {
        }

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