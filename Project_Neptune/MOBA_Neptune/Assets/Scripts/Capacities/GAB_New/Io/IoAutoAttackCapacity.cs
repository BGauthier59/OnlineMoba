using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IoAutoAttackCapacity : NewActiveCapacity
{
    public override void RequestCastCapacity(int[] targetedEntities, Vector3[] targetedPositions)
    {
        Debug.Log("This capacity is not available!");
    }

    public override void CastCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions)
    {
        throw new System.NotImplementedException();
    }

    public override void SyncCastCapacityRPC(int[] targetedEntities, Vector3[] targetedPositions)
    {
        throw new System.NotImplementedException();
    }

    public override bool TryCast()
    {
        throw new System.NotImplementedException();
    }
}
