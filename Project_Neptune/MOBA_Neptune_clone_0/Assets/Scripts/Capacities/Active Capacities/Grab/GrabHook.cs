using System;
using Capacities.Passive_Capacities;
using Entities;
using Entities.Capacities;
using Entities.Interfaces;
using GameStates;
using Photon.Pun;
using UnityEngine;

namespace Capacities.Active_Capacities.Grab
{
    public class GrabHook : MonoBehaviour
    {
        public Entity caster;
        [SerializeField] private float shootForce;

        private bool isLaunching;
        [SerializeField] private float lifeDuration;
        private float timer;

        private Vector3 backPosition;
        [SerializeField] private float backDuration;
        private float backTimer;
        private bool isComingBack;

        [SerializeField] private PassiveCapacitySO grabbedCapacitySO;

        public void Shoot()
        {
            transform.SetParent(caster.transform);
            isLaunching = true;
        }

        private void FixedUpdate()
        {
            if (!isLaunching) return;
            transform.localPosition += transform.forward * (shootForce * Time.fixedDeltaTime);
        }

        private void Update()
        {
            if (isComingBack) CheckComingBackTimer();
            else if (isLaunching) CheckLaunchingTimer();
        }

        private void CheckLaunchingTimer()
        {
            if (timer >= lifeDuration)
            {
                timer = 0;
                backPosition = transform.position;
                isComingBack = true;
                isLaunching = false;
            }
            else timer += Time.deltaTime;
        }

        private void CheckComingBackTimer()
        {
            if (backTimer >= backDuration)
            {
                isComingBack = false;
                gameObject.SetActive(false);
            }
            else
            {
                transform.position = Vector3.Lerp(backPosition, caster.transform.position, backTimer / backDuration);
                backTimer += Time.deltaTime;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            var grabable = other.gameObject.GetComponent<IGrabable>();
            if (grabable == null) return;
            
            var entity = other.gameObject.GetComponent<Entity>();
            if (entity == caster) return;
            
            var team = entity.team;

            Debug.Log(team);
            if (team == caster.team)
            {
                Debug.Log("You grabbed an ally");

                // Set passive capacity Grabbed on caster
                var capacityIndex = CapacitySOCollectionManager.GetPassiveCapacitySOIndex(grabbedCapacitySO);
                var giver = EntityCollectionManager.GetEntityIndex(entity);
                caster.AddPassiveCapacityRPC(capacityIndex, giver);
            }
            else if (team == Enums.Team.Neutral)
            {
                Debug.Log("You grabbed a wall");
                var contactPoint = other.contacts[0].point;
                var capacityIndex = CapacitySOCollectionManager.GetPassiveCapacitySOIndex(grabbedCapacitySO);
                caster.AddPassiveCapacityRPC(capacityIndex, default, contactPoint);
            }
            else
            {
                Debug.Log("You grabbed an enemy");

                // Set passive capacity Grabbed on both caster and grabable
            }

            grabable.OnGrabbed();

            gameObject.SetActive(false);
        }
    }
}