using System;
using Entities.Capacities;
using UnityEngine;

namespace Capacities.Active_Capacities
{
    [CreateAssetMenu(menuName = "Capacity/ActiveCapacitySO/Bipède AA Capacity", fileName = "Bipede_AA_SO")]
    public class BipedeAACapacitySO : ActiveCapacitySO
    {
        public BipedeAAProjectile projectilePrefab;
        public int capacityDamages;
        
        public override Type AssociatedType()
        {
            return typeof(BipedeAACapacity);
        }
    }
}
