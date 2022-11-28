using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Entities.Capacities
{
    public class DamageOnCollide : Entity
    {
        [HideInInspector] public Entity caster;
        [HideInInspector] public float damage;
        [HideInInspector] public Vector3 dir;
        [HideInInspector] public float speed;
        [SerializeField] private List<byte> effectIndex = new List<byte>();

        private void OnTriggerEnter(Collider other)
        {
            Entity entity = other.GetComponent<Entity>();

            if (entity && entity != caster)
            {
                IActiveLifeable activeLifeable = entity.GetComponent<IActiveLifeable>();
                
                if (PhotonNetwork.IsMasterClient)
                {
                    activeLifeable.DecreaseCurrentHpRPC(damage);
                    
                    gameObject.SetActive(false);
                    
                    foreach (byte index in effectIndex)
                    {
                        //TODO entity.AddPassive(index)
                    }
                }
            }
        }
    }
}


