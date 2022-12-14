using Entities;
using Entities.Capacities;
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
        private Champion grabbedChampion;
        private IStreamable streamable;

        private float initDistance;

        protected override void OnAddedEffects()
        {
            InputManager.PlayerMap.Movement.Disable();
            data = (GrabbedCapacitySO)AssociatedPassiveCapacitySO();

            var grabable = entityUnderEffect.GetComponent<IGrabable>();
            grabable?.OnGrabbed();
            grabbedChampion = (Champion)entityUnderEffect;

            duration = data.duration;
            timer = 0;

            displaceable = entityUnderEffect.GetComponent<IDisplaceable>();
            if (displaceable == null)
            {
                Debug.LogWarning("Can't displace this grabable entity?");
                return;
            }

            var pointToReach = giverEntity != null ? giverEntity.transform.position : pos;
            pointToReach.y = 1;
            initDistance = Vector3.Distance(entityUnderEffect.transform.position, pointToReach); 
            
            GameStateMachine.Instance.OnTick += MoveGrabbedEntity;
        }

        private void MoveGrabbedEntity()
        {
            var pointToReach = giverEntity != null ? giverEntity.transform.position : pos;
            pointToReach.y = 1;

            var distance = Vector3.Distance(entityUnderEffect.transform.position, pointToReach);

            var crossedDistance = Mathf.Abs(distance - initDistance);
            
            var velocity = (pointToReach - entityUnderEffect.transform.position) *
                           ((crossedDistance + .5f) * data.distanceSpeedFactor * data.speed);
            velocity.y = 0;
            displaceable.SetVelocity(velocity);

            securityTimer += 1.0 / GameStateMachine.Instance.tickRate;

            if (securityTimer >= 5)
            {
                Debug.LogWarning($"Can't reach its target at pos {pointToReach}");
                GrabbedEntityHitTarget();
            }

            if (!(distance < 1.2f)) return;
            GrabbedEntityHitTarget();
        }

        private void GrabbedEntityHitTarget()
        {
            Debug.Log("Grabbed entity hit target!");
            GameStateMachine.Instance.OnTick -= MoveGrabbedEntity;

            if (giverEntity != null && giverEntity.team == entityUnderEffect.team)
            {
                entityUnderEffect.transform.position =
                    giverEntity.transform.position - entityUnderEffect.transform.forward * .5f;
                GameStateMachine.Instance.OnTick += SetVelocityOnHookedEntity;
            }
            grabbedChampion.rb.velocity = Vector3.zero;
            
            grabbedChampion.OnUnGrabbed();

            InputManager.PlayerMap.Movement.Enable();
            streamable = entityUnderEffect.GetComponent<IStreamable>();
            streamable?.SetIsUnderStreamEffectRPC(false);

            GameStateMachine.Instance.OnTick += CheckTimer;
        }

        private void SetVelocityOnHookedEntity()
        {
            entityUnderEffect.rb.velocity = giverEntity.rb.velocity;
        }

        protected override void OnAddedFeedbackEffects()
        {
            grabbedChampion = (Champion)entityUnderEffect;
            grabbedChampion.grabLine.SetPosition(0, grabbedChampion.transform.position);
            var p = giverEntity == null ? pos : giverEntity.transform.position;
            grabbedChampion.grabLine.SetPosition(1, p);
            grabbedChampion.grabLine.enabled = true;

            GameStateMachine.Instance.OnTickFeedback += SetLineFeedback;
        }

        private void SetLineFeedback()
        {
            var initPos = grabbedChampion.transform.position;
            initPos.y = 1;
            grabbedChampion.grabLine.SetPosition(0, initPos);
            var p = giverEntity == null ? pos : giverEntity.transform.position;
            p.y = 1;
            grabbedChampion.grabLine.SetPosition(1, p);
        }

        protected override void OnRemovedEffects(Entity target)
        {
            Debug.Log("Grabbed entity is not grabbed anymore");
            if (giverEntity != null)
            {
                entityUnderEffect.rb.velocity = Vector3.zero;
                GameStateMachine.Instance.OnTick -= SetVelocityOnHookedEntity;
            }

            if(!grabbedChampion.underStreamEffect) streamable?.SetIsUnderStreamEffectRPC(true);
        }

        protected override void OnRemovedFeedbackEffects(Entity target)
        {
            grabbedChampion.grabLine.enabled = false;
            GameStateMachine.Instance.OnTickFeedback -= SetLineFeedback;
        }

        private void CheckTimer()
        {
            timer += 1.0 / GameStateMachine.Instance.tickRate;
            if (timer >= duration - .1 || entityUnderEffect.rb.velocity.magnitude != 0)
            {
                timer = 0;
                GameStateMachine.Instance.OnTick -= CheckTimer;
                entityUnderEffect.RemovePassiveCapacityByIndex(indexOfSo);
            }
        }
    }
}