using System;
using Capacities.Active_Capacities.Grab;
using Capacities.Passive_Capacities;
using Entities;
using Entities.Capacities;
using Entities.Champion;
using Entities.Interfaces;
using GameStates;
using Photon.Pun;
using UnityEditor;
using UnityEngine;

namespace Capacities.Active_Capacities
{
    public class GrabCapacity : ActiveCapacity
    {
        private GrabCapacitySO data;
        private double timer;

        private Vector3 direction;
        private LayerMask grabableLayer;
        private Vector3 casterInitPos;

        private RaycastHit hitData;
        private Champion champion;

        public override bool TryCast(int casterIndex, int[] targetsEntityIndexes, Vector3[] targetPositions)
        {
            // Set data
            champion = (Champion) caster;
            casterInitPos = GetCasterPos();
            data = (GrabCapacitySO) AssociatedActiveCapacitySO();
            grabableLayer = data.grabableLayer;
            direction = -(casterInitPos - targetPositions[0]);
            direction.y = 0;
            direction.Normalize();

            Debug.DrawRay(casterInitPos + champion.rotateParent.forward, direction * data.grabMaxDistance, Color.yellow,
                3);

            // Check conditions
            if (!Physics.Raycast(casterInitPos + champion.rotateParent.forward, direction, out var hit,
                data.grabMaxDistance, grabableLayer))
            {
                Debug.Log("Raycast for grab hit nothing!");
                return false;
            }

            hitData = hit;
            GameStateMachine.Instance.OnTick += CheckTimer;
            return true;
        }

        private void CheckTimer()
        {
            if (timer > data.delayDuration)
            {
                GameStateMachine.Instance.OnTick -= CheckTimer;
                CastGrab();
            }
            else timer += 1.0 / GameStateMachine.Instance.tickRate;
        }

        private void CastGrab()
        {
            Debug.DrawLine(casterInitPos, hitData.point, Color.red, 3);

            // We get hit IGrabable data
            var grabable = hitData.collider.gameObject.GetComponent<IGrabable>();
            if (grabable == null) return;

            // We get hit entity data
            var entity = hitData.collider.gameObject.GetComponent<Entity>();
            if (entity == caster)
            {
                Debug.LogWarning("Touched itself!");
                return;
            }

            var team = entity.team;
            var capacityIndex = CapacitySOCollectionManager.GetPassiveCapacitySOIndex(data.passiveEffect);

            if (team == caster.team)
            {
                caster.AddPassiveCapacityRPC(capacityIndex, entity.entityIndex);
            }
            else if (team == Enums.Team.Neutral)
            {
                var point = hitData.point;
                point.y = 1;
                caster.AddPassiveCapacityRPC(capacityIndex, default, point);
            }
            else
            {
                Debug.Log("You grabbed an enemy");
                var point = (entity.transform.position + caster.transform.position) * .5f;
                entity.AddPassiveCapacityRPC(capacityIndex, default, point);
                caster.AddPassiveCapacityRPC(capacityIndex, default, point);
            }
        }

        public override void PlayFeedback(int casterIndex, int[] targetsEntityIndexes, Vector3[] targetPositions)
        {
        }

        private void PlayHitEffect()
        {
            Debug.Log(caster);
            champion.grabVFX.transform.position = hitData.point;
            champion.grabVFX.Play();
            GameStateMachine.Instance.OnTickFeedback -= PlayHitEffect;
        }
    }
}