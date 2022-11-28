using System;
using UnityEngine;

namespace Entities.Inventory
{
    [CreateAssetMenu(menuName = "ItemSO/HealthModItem", fileName = "new HealthModItemSO")]
    public class HealthModItemSO : ItemSO
    {
        public int healthMod;
        
        public override Type AssociatedType()
        {
            return typeof(HealthModItem);
        }
    }
}

