using System;
using UnityEngine;

namespace Entities.Capacities
{
    [CreateAssetMenu(menuName = "Capacity/PassiveCapacitySO/Generic Passive/Slow", fileName = "new Slow")]
    public class PassiveSlowEffectSO : PassiveCapacitySO
    {
        public float slowAmount;
    
        public override Type AssociatedType()
        {
            return typeof(PassiveSlowEffect);
        }
    }
}


