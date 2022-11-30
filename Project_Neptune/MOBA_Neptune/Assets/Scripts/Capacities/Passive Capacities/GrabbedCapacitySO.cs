using System;
using Entities.Capacities;
using UnityEngine;

namespace Capacities.Passive_Capacities
{
    [CreateAssetMenu(menuName = "Capacity/PassiveCapacitySO/Grabbed", fileName = "new PassiveCapacitySO")]
    public class GrabbedCapacitySO : PassiveCapacitySO
    {
        public double duration;
        public double timer;
        public float grabStrength;
        
        public override Type AssociatedType()
        {
            return typeof(GrabbedCapacity);
        }
    }
}
