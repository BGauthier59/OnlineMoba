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

        public override bool TryCast(int casterIndex, int[] targetsEntityIndexes, Vector3[] targetPositions)
        {
            // Check condition
            if (onCooldown)
            {
                Debug.LogWarning("You're on a cooldown for grab capacity!");
                return false;
            }
            
            data = (GrabCapacitySO)AssociatedActiveCapacitySO();
            casterInitPos = GetCasterPos();
            direction = -(casterInitPos - targetPositions[0]);
            direction.y = 0;
            direction = direction.normalized;
            grabableLayer = data.grabableLayer;
            Debug.DrawRay(casterInitPos, direction * 10, Color.magenta, 3f);

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
            else
            {
                timer += 1.0 / GameStateMachine.Instance.tickRate;
            }
        }

        private void CastGrab()
        {
            Debug.Log("Casting grab!");
            Debug.DrawRay(casterInitPos, direction * data.grabMaxDistance, Color.yellow, 3);
            var champion = (Champion)caster;

            if (!Physics.Raycast(casterInitPos + champion.rotateParent.forward, direction, out var hit,
                    data.grabMaxDistance, grabableLayer)) return;
            
            Debug.DrawLine(casterInitPos, hit.point, Color.red, 3);

            // We get hit IGrabable data
            var grabable = hit.collider.gameObject.GetComponent<IGrabable>();
            if (grabable == null)
            {
                return;
            }

            // We get hit entity data
            var entity = hit.collider.gameObject.GetComponent<Entity>();
            if (entity == caster)
            {
                Debug.LogWarning("Touched itself!");
                return;
            }
            Debug.Log($"You hit {entity.name}");

            champion.grabVFX.transform.position = hit.point;
            champion.grabVFX.Play();

            var team = entity.team;
            var capacityIndex = CapacitySOCollectionManager.GetPassiveCapacitySOIndex(data.passiveEffect);

            if (team == caster.team)
            {
                caster.AddPassiveCapacityRPC(capacityIndex, entity.entityIndex);
            }
            else if (team == Enums.Team.Neutral)
            {
                var point = hit.point;
                point.y = 1;
                caster.AddPassiveCapacityRPC(capacityIndex, default, point);
            }
            else
            {
                Debug.Log("You grabbed an enemy");
                // Set passive capacity Grabbed on both caster and grabable
            }
        }

        public override void PlayFeedback(int casterIndex, int[] targetsEntityIndexes, Vector3[] targetPositions)
        {
            
        }
    }
}