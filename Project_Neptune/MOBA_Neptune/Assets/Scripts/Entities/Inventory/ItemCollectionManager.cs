using System;
using System.Collections.Generic;
using Photon.Pun;

namespace Entities.Inventory
{
    public class ItemCollectionManager : MonoBehaviourPun
    {
        public List<ItemSO> allItemSOs = new List<ItemSO>();
        public readonly List<ItemSO> currentItems = new List<ItemSO>();
        public static ItemCollectionManager Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                DestroyImmediate(this);
                return;
            }
            Instance = this;
        }

        public void LinkCapacityIndexes()
        {
            for (byte index = 0; index < allItemSOs.Count; index++)
            {
                allItemSOs[index].SetIndexes(index);
            }
        }

        public Item CreateItem(byte soIndex,Entity entity)
        {
            var inventory = entity.GetComponent<IInventoryable>();
            if (inventory == null) return null;
            if(soIndex>= allItemSOs.Count) return null;
            var so = allItemSOs[soIndex];
            Item item;
            if (so.consumable)
            {
                item = inventory.GetItemOfSo(soIndex);
                if (item != null)
                {
                    item.consumable = so.consumable;
                    return item;
                }
            }
            item = (Item) Activator.CreateInstance(allItemSOs[soIndex].AssociatedType());
            item.consumable = so.consumable;
            item.indexOfSOInCollection = soIndex;
            
            return item;
        }
        
        public ItemSO GetItemSObyIndex(byte index)
        {
            return allItemSOs[index];
        }

        public bool IsInCurrentItems(ItemSO itemSO)
        {
            return currentItems.Contains(itemSO);
        }

        public bool IsInCurrentItems(Item item)
        {
            return IsInCurrentItems(item.AssociatedItemSO());
        }

        public bool IsInCurrentItems(byte itemSoIndex)
        {
            if (itemSoIndex >= allItemSOs.Count) return false;
            return IsInCurrentItems(allItemSOs[itemSoIndex]);
        }

        public void TryAddToCurrentItems(byte itemSoIndex)
        {
            if (itemSoIndex >= allItemSOs.Count) return;
            photonView.RPC("AddToCurrentItemsRPC",RpcTarget.All,itemSoIndex);
        }

        [PunRPC] public void AddToCurrentItemsRPC(byte itemSoIndex)
        {
            if (itemSoIndex >= allItemSOs.Count) return;
            currentItems.Add(allItemSOs[itemSoIndex]);
        }

        public void TryRemoveFromCurrentItems(byte itemSoIndex)
        {
            if (itemSoIndex >= allItemSOs.Count) return;
            photonView.RPC("RemoveToCurrentItemsRPC",RpcTarget.All,itemSoIndex);
        }
        
        [PunRPC] public void RemoveToCurrentItemsRPC(byte itemSoIndex)
        {
            if (itemSoIndex >= allItemSOs.Count) return;
            var itemSo = allItemSOs[itemSoIndex];
            if(currentItems.Contains(itemSo)) currentItems.Remove(itemSo);
        }
    }
}

