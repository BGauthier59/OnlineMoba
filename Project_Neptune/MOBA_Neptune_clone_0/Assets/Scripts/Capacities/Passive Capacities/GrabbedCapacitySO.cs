using System;
using UnityEngine;

namespace Capacities.Passive_Capacities
{
    [CreateAssetMenu(menuName = "Capacity/PassiveCapacitySO/Grabbed", fileName = "new PassiveCapacitySO")]
    public class GrabbedCapacitySO : PassiveCapacitySO
    {
        public double duration;
        public float speed;
        
        public override Type AssociatedType()
        {
            return typeof(GrabbedCapacity);
        }
    }
}
