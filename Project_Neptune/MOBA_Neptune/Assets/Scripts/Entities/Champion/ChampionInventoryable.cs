using System.Collections.Generic;
using System.Linq;
using Entities.Capacities;
using Entities.Inventory;
using GameStates;
using Photon.Pun;
using UnityEngine;

namespace Entities.Champion
{
    public partial class Champion : IInventoryable
    {
        [SerializeReference] public List<Item> items = new List<Item>();

        public Item[] GetItems()
        {
            return items.ToArray();
        }

        public Item GetItem(int index)
        {
            if (index < 0 || index >= 3) return null;
            return items[index];
        }

        public Item GetItemOfSo(int soIndex)
        {
            return items.FirstOrDefault(item => item.indexOfSOInCollection == soIndex);
        }

        public void RequestAddItem(byte index)
        {
            photonView.RPC("AddItemRPC",RpcTarget.MasterClient,index);
        }

        [PunRPC]
        public void AddItemRPC(byte index)
        {
            var itemSo = ItemCollectionManager.Instance.GetItemSObyIndex(index);
            if (itemSo.consumable)
            {
                var contains = false;
                foreach (var item in items.Where(item => item.indexOfSOInCollection == index))
                {
                    contains = true;
                }
                if(!contains && items.Count>=3) return;
                photonView.RPC("SyncAddItemRPC",RpcTarget.All, index);
                return;
            }
            if(items.Count>=3) return;
            photonView.RPC("SyncAddItemRPC",RpcTarget.All, index);
        }

        [PunRPC]
        public void SyncAddItemRPC(byte index)
        {
            var item = ItemCollectionManager.Instance.CreateItem(index, this);
            if(item == null) return;
            if(!items.Contains(item)) items.Add(item);
            if (PhotonNetwork.IsMasterClient)
            {
                item.OnItemAddedToInventory(this);
                OnAddItem?.Invoke(index);
            }

            item.OnItemAddedToInventoryFeedback(this);
            OnAddItemFeedback?.Invoke(index);
        }
        
        public event GlobalDelegates.ByteDelegate OnAddItem;
        public event GlobalDelegates.ByteDelegate OnAddItemFeedback;
        
        /// <param name="index">index of Item in this entity's inventory (not in item Collection)</param>
        public void RequestRemoveItem(byte index)
        {
            photonView.RPC("RemoveItemRPC",RpcTarget.MasterClient,index);
        }

        /// <param name="item">Item to remove from this entity's inventory</param>
        public void RequestRemoveItem(Item item)
        {
            if(!items.Contains(item)) return;
            RequestRemoveItem((byte)items.IndexOf(item));
        }

        [PunRPC]
        public void RemoveItemRPC(byte index)
        {
            photonView.RPC("SyncRemoveItemRPC",RpcTarget.All,index);
        }

        public void RemoveItemRPC(Item item)
        {
            if(!items.Contains(item)) return;
            var index = items.IndexOf(item);
            RemoveItemRPC((byte)index);
        }

        [PunRPC]
        public void SyncRemoveItemRPC(byte index)
        {
            if(index >= items.Count) return;
            var item = items[index];
            items.Remove(item);
            if (PhotonNetwork.IsMasterClient)
            {
                item.OnItemRemovedFromInventory(this);
                OnRemoveItem?.Invoke(index);
            }
            
            item.OnItemRemovedFromInventoryFeedback(this);
            OnRemoveItemFeedback?.Invoke(index);
        }
        public event GlobalDelegates.ByteDelegate OnRemoveItem;
        public event GlobalDelegates.ByteDelegate OnRemoveItemFeedback;
        
        public void RequestActivateItem(byte itemIndexInInventory,int[] selectedEntities,Vector3[] positions)
        {
            if(itemIndexInInventory >= items.Count) return;
            photonView.RPC("ActivateItemRPC",RpcTarget.MasterClient,itemIndexInInventory,selectedEntities,positions);
        }

        [PunRPC]
        public void ActivateItemRPC(byte itemIndexInInventory,int[] selectedEntities,Vector3[] positions)
        {
            if(itemIndexInInventory >= items.Count) return;
            var item = items[itemIndexInInventory];
            if(item == null) return;
            
            var successesActives = new bool[item.AssociatedItemSO().activeCapacitiesIndexes.Length];
            var bytes = item.AssociatedItemSO().activeCapacitiesIndexes;
            for (var i = 0; i < bytes.Length; i++)
            {
                var capacityIndex = bytes[i];
                var activeCapacity = CapacitySOCollectionManager.CreateActiveCapacity(capacityIndex, this);
                successesActives[i] = activeCapacity.TryCast(entityIndex, selectedEntities, positions);
            }
            items[itemIndexInInventory].OnItemActivated(selectedEntities,positions);
            OnActivateItem?.Invoke(itemIndexInInventory,selectedEntities,positions);
            photonView.RPC("SyncActivateItemRPC",RpcTarget.All,itemIndexInInventory,selectedEntities,positions,successesActives.ToArray());
            

        }

        [PunRPC]
        public void SyncActivateItemRPC(byte itemIndexInInventory,int[] selectedEntities,Vector3[] positions,bool[] castSuccess)
        {
            if(itemIndexInInventory >= items.Count) return;
            var item = items[itemIndexInInventory];
            if(items[itemIndexInInventory] == null) return;
            var bytes = item.AssociatedItemSO().activeCapacitiesIndexes;
            for (var index = 0; index < bytes.Length; index++)
            {
                var capacityIndex = bytes[index];
                var activeCapacity = CapacitySOCollectionManager.CreateActiveCapacity(capacityIndex, this);
                if(castSuccess[index]) activeCapacity.PlayFeedback(entityIndex,selectedEntities,positions);
            }
            items[itemIndexInInventory].OnItemActivatedFeedback(selectedEntities,positions);
            OnActivateItemFeedback?.Invoke(itemIndexInInventory,selectedEntities,positions,castSuccess);
        }

        public event GlobalDelegates.ByteIntArrayVector3ArrayDelegate OnActivateItem;
        public event GlobalDelegates.ByteIntArrayVector3ArrayBoolArrayDelegate OnActivateItemFeedback;
    }
}