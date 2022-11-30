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

        private void OnTriggerEnter(Collider other)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            var grabable = other.GetComponent<IGrabable>();

            if (grabable == null) return;
            
            var entity = other.GetComponent<Entity>();
            var team = entity.team;

            if (team == caster.team || team == Enums.Team.Neutral)
            {
                Debug.Log("You grabbed an ally or a wall");

                // Set passive capacity Grabbed on caster
                caster.grabbingEntity = entity;
                var index = CapacitySOCollectionManager.GetPassiveCapacitySOIndex(grabbedCapacitySO);
                caster.AddPassiveCapacityRPC(index);
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