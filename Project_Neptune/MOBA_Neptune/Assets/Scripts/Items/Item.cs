using System.Collections.Generic;
using Entities.Capacities;
using Photon.Pun;
using UnityEngine;

namespace Entities.Inventory
{
    [System.Serializable]
    public abstract class Item
    {
        public bool consumable;
        
        public int count;
        public float timer;
        public Entity entityOfInventory;
        public IInventoryable inventory;

        public byte indexOfSOInCollection;

        public ItemSO AssociatedItemSO()
        {
            return ItemCollectionManager.Instance.GetItemSObyIndex(indexOfSOInCollection);
        }

        public void OnItemAddedToInventory(Entity entity)
        {
            if (consumable) count++;
            entityOfInventory = entity;
            inventory = entityOfInventory.GetComponent<IInventoryable>();
            OnItemAddedEffects(entity);
            foreach (var index in AssociatedItemSO().passiveCapacitiesIndexes)
            {
                entityOfInventory.AddPassiveCapacityRPC(index);
            }
        }

        protected abstract void OnItemAddedEffects(Entity entity);

        public void OnItemAddedToInventoryFeedback(Entity entity)
        {
            entityOfInventory = entity;
            inventory = entityOfInventory.GetComponent<IInventoryable>();
            OnItemAddedEffectsFeedback(entity);
        }

        protected abstract void OnItemAddedEffectsFeedback(Entity entity);
        
        public void OnItemRemovedFromInventory(Entity entity)
        {
            OnItemRemovedEffects(entity);
            foreach (var index in AssociatedItemSO().passiveCapacitiesIndexes)
            {
                entityOfInventory.RemovePassiveCapacityByIndex(index);
            }
        }

        protected abstract void OnItemRemovedEffects(Entity entity);

        public void OnItemRemovedFromInventoryFeedback(Entity entity)
        {
            OnItemRemovedEffectsFeedback(entity);
        }

        protected abstract void OnItemRemovedEffectsFeedback(Entity entity);
        
        public virtual void OnItemActivated(int[] targets, Vector3[] positions)
        {
            if(!consumable) return;
            count--;
        }
        
        public virtual void OnItemActivatedFeedback(int[] targets, Vector3[] positions)
        {
            if(!consumable) return;
            if (!PhotonNetwork.IsMasterClient) count--;
            if(count > 0 || !PhotonNetwork.IsMasterClient) return;
            inventory.RemoveItemRPC(this);
        }
    }
}