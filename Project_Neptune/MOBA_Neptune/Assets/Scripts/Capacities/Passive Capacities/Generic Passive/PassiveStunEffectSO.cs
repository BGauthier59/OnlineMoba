using System;
using UnityEngine;

namespace Entities.Capacities
{
    [CreateAssetMenu(menuName = "Capacity/PassiveCapacitySO/Generic Passive/Stun", fileName = "new Stun")]
    public class PassiveStunEffectSO : PassiveCapacitySO
    {
        public float stunDuration;
    
        public override Type AssociatedType()
        {
            return typeof(PassiveStunEffect);
        }
    }
}
