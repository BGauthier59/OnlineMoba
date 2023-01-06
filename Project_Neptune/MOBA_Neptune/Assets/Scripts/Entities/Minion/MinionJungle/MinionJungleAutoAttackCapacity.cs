using System;
using Entities;
using Entities.Champion;
using Entities.Minion.MinionJungle;
using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;

public class MinionJungleAutoAttackCapacity : NewActiveCapacity
{
    [Space] [Header("Capacity Stats")]
    [SerializeField] private int capacityDamage;
    [SerializeField] private Vector3 casterPos;
    [SerializeField] private Vector3 targetPos;

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        capacityDamage = GetComponent<MinionJungle>().autoAttackDamage;
    }

    public override void RequestCastCapacity(int[] targetedEntities, Vector3[] targetedPositions)
    {
        photonView.RPC("CastMinionJungleAutoAttackCapacityRPC", RpcTarget.MasterClient, targetedEntities, targetedPositions);
    }

    [PunRPC] [UsedImplicitly]
    public void CastMinionJungleAutoAttackCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions)
    {
        photonView.RPC("SyncDataJungleAutoAttackCapacityRPC", RpcTarget.All);
        
        foreach (var c in targetedEntities)
        {
            var damageable = EntityCollectionManager.GetEntityByIndex(c).GetComponent<IDamageable>();
            damageable?.DecreaseCurrentHpRPC(capacityDamage, caster.entityIndex);
            Debug.LogFormat($"{caster.name} attacked {EntityCollectionManager.GetEntityByIndex(c)} !");
            Debug.LogFormat($"{EntityCollectionManager.GetEntityByIndex(c)} current health : {EntityCollectionManager.GetEntityByIndex(c).GetComponent<Champion>().currentHp}");
        }
    }

    [PunRPC] [UsedImplicitly]
    public void SyncDataJungleAutoAttackCapacityRPC()
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