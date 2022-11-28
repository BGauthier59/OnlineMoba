using System;
using UnityEngine;

namespace Entities.Capacities
{
    [CreateAssetMenu(menuName = "Capacity/PassiveCapacitySO/Generic Passive/Knockback", fileName = "new Knockback")]
    public class PassiveKnockbackEffectSO : PassiveCapacitySO
    {

        public float knockbackForce;
        public override Type AssociatedType()
        {
            return typeof(PassiveKnockbackEffect);
        }
    }
}


