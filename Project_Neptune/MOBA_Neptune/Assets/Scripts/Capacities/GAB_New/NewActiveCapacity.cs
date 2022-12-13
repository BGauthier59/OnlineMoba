using System.Collections;
using System.Collections.Generic;
using Entities;
using GameStates;
using Photon.Pun;
using UnityEngine;

public abstract class NewActiveCapacity : MonoBehaviourPun
{
    public Entity caster;
    
    public bool canCastCapacity;
    public double cooldownDuration;
    protected double cooldownTimer;
    
    public abstract void RequestCastCapacity(int[] targetedEntities, Vector3[] targetedPositions);

    public abstract void CastCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions);

    public abstract void SyncCastCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions);

    public abstract bool TryCast();
    
    protected Vector3 GetCasterPos()
    {
        var casterPos = caster.transform.position;
        casterPos.y = 1;
        return casterPos;
    }

    protected void StartCooldown()
    {
        photonView.RPC("SyncCanCastGrabCapacityRPC", RpcTarget.All, false);
    }
}
