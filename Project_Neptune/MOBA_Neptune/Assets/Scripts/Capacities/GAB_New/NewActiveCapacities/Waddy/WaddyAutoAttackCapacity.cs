using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaddyAutoAttackCapacity : NewActiveCapacity
{
    public override void RequestCastCapacity(int[] targetedEntities, Vector3[] targetedPositions)
    {
        Debug.Log("This capacity is not available!");
    }

    public void CastCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions)
    {
        throw new System.NotImplementedException();
    }

    public void SyncCastCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions)
    {
        throw new System.NotImplementedException();
    }

    public override bool TryCast()
    {
        throw new System.NotImplementedException();
    }

    protected override void StartCooldown()
    {
        throw new System.NotImplementedException();
    }

    protected override void TimerCooldown()
    {
        throw new System.NotImplementedException();
    }
}
