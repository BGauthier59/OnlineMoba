using System;
using UnityEngine;

namespace Entities.Capacities
{
    [CreateAssetMenu(menuName = "Capacity/ActiveCapacitySO/Dark Matter", fileName = "new Dark Matter")]
    public class ActiveDarkMatterSO : ActiveCapacitySO
    {
        public float zoneRadius;
        public float damageAmount;
        public float delay;
    
        public override Type AssociatedType()
        {
            return typeof(ActiveDarkMatter);
        }
    }
}


