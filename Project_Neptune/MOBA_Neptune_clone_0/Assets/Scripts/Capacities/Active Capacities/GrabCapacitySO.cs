using System;
using Capacities.Active_Capacities.Grab;
using Entities.Capacities;
using UnityEngine;

namespace Capacities.Active_Capacities
{
    [CreateAssetMenu(menuName = "Capacity/ActiveCapacitySO/Grab Capacity", fileName = "Grab Capacity SO")]
    public class GrabCapacitySO : ActiveCapacitySO
    {
        public GrabHook grabHookPrefab;
        public Vector3 shootOriginOffset;
        
        public override Type AssociatedType()
        {
            return typeof(GrabCapacity);
        }
    }
}
