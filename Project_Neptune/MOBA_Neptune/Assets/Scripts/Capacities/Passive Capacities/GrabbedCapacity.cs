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
        private Champion championUnderEffect;
        private IStreamable streamable;

        private bool attached;

        private float initDistance;

        protected override void OnAddedEffects()
        {
            InputManager.PlayerMap.Movement.Disable();
            data = (GrabbedCapacitySO)AssociatedPassiveCapacitySO();

            var grabable = entityUnderEffect.GetComponent<IGrabable>();
            grabable?.OnGrabbed();
            championUnderEffect = (Champion)entityUnderEffect;

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

            championUnderEffect.rb.velocity = Vector3.zero;
            attached = true;

            InputManager.PlayerMap.Movement.Enable();
            championUnderEffect.OnMove += OnMoveWhileAttached;

            streamable = entityUnderEffect.GetComponent<IStreamable>();
            //streamable?.SetIsUnderStreamEffectRPC(false);

            GameStateMachine.Instance.OnTick += CheckTimer;
        }

        private void SetVelocityOnHookedEntity()
        {
            entityUnderEffect.rb.velocity = giverEntity.rb.velocity;
        }

        protected override void OnAddedFeedbackEffects()
        {
            championUnderEffect = (Champion)entityUnderEffect;
            championUnderEffect.grabLine.SetPosition(0, championUnderEffect.transform.position);
            var p = giverEntity == null ? pos : giverEntity.transform.position;
            championUnderEffect.grabLine.SetPosition(1, p);
            championUnderEffect.grabLine.enabled = true;

            GameStateMachine.Instance.OnTickFeedback += SetLineFeedback;
        }

        private void SetLineFeedback()
        {
            var initPos = championUnderEffect.transform.position;
            initPos.y = 1;
            championUnderEffect.grabLine.SetPosition(0, initPos);
            var p = giverEntity == null ? pos : giverEntity.transform.position;
            p.y = 1;
            championUnderEffect.grabLine.SetPosition(1, p);
        }

        protected override void OnRemovedEffects(Entity target)
        {
            Debug.Log("Grabbed entity is not grabbed anymore");
            if (giverEntity != null)
            {
                entityUnderEffect.rb.velocity = Vector3.zero;
                GameStateMachine.Instance.OnTick -= SetVelocityOnHookedEntity;
            }

            //if (!championUnderEffect.underStreamEffect) streamable?.SetIsUnderStreamEffectRPC(true);
        }

        protected override void OnRemovedFeedbackEffects(Entity target)
        {
            championUnderEffect.grabLine.enabled = false;
            GameStateMachine.Instance.OnTickFeedback -= SetLineFeedback;
        }

        private void CheckTimer()
        {
            timer += 1.0 / GameStateMachine.Instance.tickRate;
            if (timer >= duration - .1 || !attached)

            {
                championUnderEffect.OnUnGrabbed();
                attached = false;

                timer = 0;
                GameStateMachine.Instance.OnTick -= CheckTimer;
                entityUnderEffect.RemovePassiveCapacityByIndex(indexOfSo);
            }
        }

        private void OnMoveWhileAttached()
        {
            Debug.Log("Unattached!");
            championUnderEffect.OnUnGrabbed();
            attached = false;
            championUnderEffect.OnMove -= OnMoveWhileAttached;
        }
    }
}