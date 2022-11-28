using System;
using UnityEngine;

namespace Entities.Capacities
{
    [CreateAssetMenu(menuName = "Capacity/PassiveCapacitySO/Generic Passive/Dot", fileName = "new Dot")]
    public class PassiveDotEffectSO : PassiveCapacitySO
    {
        public float damageTickSpeed;
        public float duration;
        public float damage;
        
        public override Type AssociatedType()
        {
            return typeof(PassiveDotEffect);
        }
    }
}

