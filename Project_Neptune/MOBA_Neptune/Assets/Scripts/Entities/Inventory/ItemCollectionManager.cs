using System;
using System.Collections.Generic;
using Photon.Pun;

namespace Entities.Inventory
{
    public class ItemCollectionManager : MonoBehaviourPun
    {
        public List<ItemSO> allItemSOs = new List<ItemSO>();
        public static readonly List<ItemSO> allItems = new List<ItemSO>();
        public static readonly List<ItemSO> currentItems = new List<ItemSO>();
        public static PhotonView view;

        private void Start()
        {
            view = photonView;
            SetAllItems();
        }

        private void SetAllItems()
        {
            allItems.Clear();
            foreach (var itemSO in allItemSOs)
            {
                allItems.Add(itemSO);
            }
        }

        public static void LinkCapacityIndexes()
        {
            foreach (var itemSO in allItems)
            {
                itemSO.SetIndexes();
            }
        }

        public static Item CreateItem(byte soIndex,Entity entity)
        {
            var inventory = entity.GetComponent<IInventoryable>();
            if (inventory == null) return null;
            if(soIndex>= allItems.Count) return null;
            var so = allItems[soIndex];
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
            item = (Item) Activator.CreateInstance(allItems[soIndex].AssociatedType());
            item.consumable = so.consumable;
            item.indexOfSOInCollection = soIndex;
            
            return item;
        }
        
        public static ItemSO GetItemSObyIndex(byte index)
        {
            return allItems[index];
        }

        public static bool IsInCurrentItems(ItemSO itemSO)
        {
            return currentItems.Contains(itemSO);
        }

        public static bool IsInCurrentItems(Item item)
        {
            return IsInCurrentItems(item.AssociatedItemSO());
        }

        public static bool IsInCurrentItems(byte itemSoIndex)
        {
            if (itemSoIndex >= allItems.Count) return false;
            return IsInCurrentItems(allItems[itemSoIndex]);
        }

        public static void TryAddToCurrentItems(byte itemSoIndex)
        {
            if (itemSoIndex >= allItems.Count) return;
            view.RPC("AddToCurrentItemsRPC",RpcTarget.All,itemSoIndex);
        }

        [PunRPC] public static void AddToCurrentItemsRPC(byte itemSoIndex)
        {
            if (itemSoIndex >= allItems.Count) return;
            currentItems.Add(allItems[itemSoIndex]);
        }

        public static void TryRemoveFromCurrentItems(byte itemSoIndex)
        {
            if (itemSoIndex >= allItems.Count) return;
            view.RPC("RemoveToCurrentItemsRPC",RpcTarget.All,itemSoIndex);
        }
        
        [PunRPC] public static void RemoveToCurrentItemsRPC(byte itemSoIndex)
        {
            if (itemSoIndex >= allItems.Count) return;
            var itemSo = allItems[itemSoIndex];
            if(currentItems.Contains(itemSo)) currentItems.Remove(itemSo);
        }
    }
}

