using System;
using Capacities.Active_Capacities.Grab;
using Capacities.Passive_Capacities;
using Entities.Capacities;
using UnityEngine;

namespace Capacities.Active_Capacities
{
    [CreateAssetMenu(menuName = "Capacity/ActiveCapacitySO/Grab Capacity", fileName = "Grab Capacity SO")]
    public class GrabCapacitySO : ActiveCapacitySO
    {
        public float grabMaxDistance;
        public double delayDuration;
        public LayerMask grabableLayer;
        public GrabbedCapacitySO passiveEffect;

        public override Type AssociatedType()
        {
            return typeof(GrabCapacity);
        }
    }
}
