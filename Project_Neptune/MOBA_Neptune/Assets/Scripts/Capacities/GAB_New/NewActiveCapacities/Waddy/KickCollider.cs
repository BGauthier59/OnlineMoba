using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SearchService;

public class KickCollider : MonoBehaviour
{
    public uint damage;
    public Enums.Team team;
    public Entity casterEntity;
    [SerializeField] private ParticleSystem impactVfx;
    
    private void OnTriggerEnter(Collider other)
    {
        if (team == Enums.Team.Neutral) team = casterEntity.team;
        
        if (!PhotonNetwork.IsMasterClient) return;

        var entity = other.GetComponent<Entity>();
        if (!entity) return;
        if (entity.team == team) return;

        var damageable = other.GetComponent<IDamageable>();
        damageable?.DecreaseCurrentHpRPC(damage, casterEntity.entityIndex);
        impactVfx.transform.position = other.transform.position;
        impactVfx.Play();
    }
}
