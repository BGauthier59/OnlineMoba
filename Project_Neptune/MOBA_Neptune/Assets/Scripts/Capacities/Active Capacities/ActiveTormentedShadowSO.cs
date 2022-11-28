using System;
using UnityEngine;

namespace Entities.Capacities
{
    [CreateAssetMenu(menuName = "Capacity/ActiveCapacitySO/Tormented Shadow", fileName = "new Tormented Shadow")]
    public class ActiveTormentedShadowSO : ActiveCapacitySO
    {
        public float zoneRadius;
        public float damageAmount;
        public float tickDamage;
        public float activeDuration;
        
        public override Type AssociatedType()
        {
            return typeof(ActiveTormentedShadow);
        }
    }
}


