using System;
using Entities;
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

    [PunRPC] [UsedImplicitly]
    public void CastMinionJungleAutoAttackCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions)
    {
        photonView.RPC("SyncDataIoAutoAttackCapacityRPC", RpcTarget.All, targetedEntities, targetedPositions[0]);
        
        foreach (var c in targetedEntities)
        {
            var damageable = EntityCollectionManager.GetEntityByIndex(c).GetComponent<IDamageable>();
            damageable?.DecreaseCurrentHpRPC(damage, caster.entityIndex);
        }
    }

    [PunRPC] [UsedImplicitly]
    public void SyncDataJungleAutoAttackCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions)
    {
        caster = GetComponent<Entity>();
        casterPos = GetCasterPos();
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