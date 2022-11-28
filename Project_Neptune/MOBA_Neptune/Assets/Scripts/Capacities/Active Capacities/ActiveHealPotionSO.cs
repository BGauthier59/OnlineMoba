using System;
using UnityEngine;

namespace Entities.Capacities
{
    [CreateAssetMenu(menuName = "Capacity/ItemCapacity/Heal Potion", fileName = "new Heal Potion")]
    public class ActiveHealPotionSO : ActiveCapacitySO
    {
        public float healAmount;

        public override Type AssociatedType()
        {
            return typeof(ActiveHealPotion);
        }
    }
}

