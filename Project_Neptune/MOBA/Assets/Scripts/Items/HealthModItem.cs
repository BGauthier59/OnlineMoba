using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Inventory
{
    public class HealthModItem : Item
    {
        private IActiveLifeable lifeable;

        protected override void OnItemAddedEffects(Entity entity)
        {
            lifeable = entity.GetComponent<IActiveLifeable>();
            lifeable?.IncreaseMaxHpRPC(((HealthModItemSO)AssociatedItemSO()).healthMod);
        }

        protected override void OnItemAddedEffectsFeedback(Entity entity)
        {
            Debug.Log($"Gained {((HealthModItemSO)AssociatedItemSO()).healthMod} hp");
        }

        protected override void OnItemRemovedEffects(Entity entity)
        {
            lifeable?.DecreaseMaxHpRPC(((HealthModItemSO)AssociatedItemSO()).healthMod);
        }

        protected override void OnItemRemovedEffectsFeedback(Entity entity)
        {
            Debug.Log($"Removed {((HealthModItemSO)AssociatedItemSO()).healthMod} hp");
        }
    }
}