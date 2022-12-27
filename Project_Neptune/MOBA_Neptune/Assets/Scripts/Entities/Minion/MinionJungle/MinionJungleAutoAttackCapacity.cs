using System;
using Entities;
using Entities.Champion;
using GameStates;
using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;

public class MinionJungleAutoAttackCapacity : NewActiveCapacity
{
    public float damage;
    [SerializeField] private Vector3 casterPos;
    [SerializeField] private Vector3 targetPos;

    public override void RequestCastCapacity(int[] targetedEntities, Vector3[] targetedPositions)
    {
        photonView.RPC("CastMinionJungleAutoAttackCapacityRPC", RpcTarget.MasterClient, targetedEntities, targetedPositions);
    }

    [PunRPC]
    public void CastMinionJungleAutoAttackCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions)
    {
        photonView.RPC("SyncDataJungleAutoAttackCapacityRPC", RpcTarget.All, targetedEntities, targetedPositions);
        
        foreach (var c in targetedEntities)
        {
            var damageable = EntityCollectionManager.GetEntityByIndex(c).GetComponent<IDamageable>();
            damageable?.DecreaseCurrentHpRPC(damage, caster.entityIndex);
            Debug.LogFormat($"{caster.name} attacked {EntityCollectionManager.GetEntityByIndex(c)} !");
            Debug.LogFormat($"{EntityCollectionManager.GetEntityByIndex(c)} current health : {EntityCollectionManager.GetEntityByIndex(c).GetComponent<Champion>().currentHp}");
        }
    }

    [PunRPC]
    public void SyncDataJungleAutoAttackCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions)
    {
        caster = GetComponent<Entity>();
    }
    
    public override bool TryCast()
    {
        return true;
    }

    protected override void StartCooldown()
    {
    }

    protected override void TimerCooldown()
    {
    }
}