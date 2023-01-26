using System;
using Entities;
using Entities.Champion;
using Entities.Minion.MinionJungle;
using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;

public class MinionJungleAutoAttackCapacity : NewActiveCapacity
{
    [Space] [Header("Capacity Stats")] [SerializeField]
    private int capacityDamage;

    public GameObject autoAttackVfx;
    [SerializeField] private Vector3 casterPos;
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private LayerMask capacityLayerMask;
    
    public new void Start() { }

    public override void RequestCastCapacity(int[] targetedEntities, Vector3[] targetedPositions)
    {
        photonView.RPC("CastMinionJungleAutoAttackCapacityRPC", RpcTarget.MasterClient, targetedEntities, targetedPositions);
    }

    [PunRPC]
    [UsedImplicitly]
    public void CastMinionJungleAutoAttackCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions)
    {
        photonView.RPC("SyncDataJungleAutoAttackCapacityRPC", RpcTarget.All);
        
        // Calcul de la capa
        var colliders = Physics.OverlapSphere(targetedPositions[0], 1.5f, capacityLayerMask);

        foreach (var entityHit in colliders)
        {
            Champion thisChampion = entityHit.GetComponent<Champion>();

            if (thisChampion && entityHit.GetComponent<Entity>() != caster)
            {
                var damageable = thisChampion.GetComponent<IDamageable>();
                damageable?.DecreaseCurrentHpRPC(capacityDamage, caster.entityIndex);
            }
        }
    }

    [PunRPC]
    [UsedImplicitly]
    public void SyncDataJungleAutoAttackCapacityRPC()
    {
        caster = GetComponent<Entity>();
        
    }

    public override bool TryCast()
    {
        return true;
    }

    protected override void StartCooldown() { }

    protected override void TimerCooldown() { }

    public override void RequestSetPreview(bool active) { }

    public override void Update() { }

    public override void UpdatePreview() { }
}