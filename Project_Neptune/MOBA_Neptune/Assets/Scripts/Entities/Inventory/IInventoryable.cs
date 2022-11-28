using UnityEngine;

namespace Entities.Inventory
{
    public interface IInventoryable
    {
        /// <returns>The items in the inventory of the entity</returns>
        public Item[] GetItems();
        /// <returns>The item in the inventory of the entity at the given index, can be null</returns>
        public Item GetItem(int index);
        public Item GetItemOfSo(int soIndex);

        /// <summary>
        /// Sends an RPC to the master to add an item to the entity's inventory.
        /// </summary>
        /// <param name="index">the index of the item on the ItemCollectionManager</param>
        public void RequestAddItem(byte index);
        /// <summary>
        /// Adds an item to the entity's inventory.
        /// </summary>
        /// <param name="index">the index of the item on the ItemCollectionManager</param>
        public void AddItemRPC(byte index);
        /// <summary>
        /// Sends an RPC to all clients to add an item to the entity's inventory.
        /// </summary>
        /// <param name="index">the index of the item on the ItemCollectionManager</param>
        public void SyncAddItemRPC(byte index);

        public event GlobalDelegates.ByteDelegate OnAddItem;
        public event GlobalDelegates.ByteDelegate OnAddItemFeedback;
        
        /// <summary>
        /// Sends an RPC to the master to remove an item from the entity's inventory.
        /// </summary>
        /// <param name="index">the index of the item on the entity's item list</param>
        public void RequestRemoveItem(byte index);
        /// <summary>
        /// Sends an RPC to the master to remove an item from the entity's inventory.
        /// </summary>
        /// <param name="item">the item on the entity's item list</param>
        public void RequestRemoveItem(Item item);
        /// <summary>
        /// Removes an item from the entity's inventory.
        /// </summary>
        /// <param name="index">the index of the item on the entity's item list</param>
        public void RemoveItemRPC(byte index);
        public void RemoveItemRPC(Item item);
        /// <summary>
        /// Sends an RPC to all clients to remove an item from the entity's inventory.
        /// </summary>
        /// <param name="index">the index of the item on the entity's item list</param>
        public void SyncRemoveItemRPC(byte index);
        public event GlobalDelegates.ByteDelegate OnRemoveItem;
        public event GlobalDelegates.ByteDelegate OnRemoveItemFeedback;
        
        /// <summary>
        /// Sends an RPC to the master to activate an item in the entity's inventory.
        /// </summary>
        /// <param name="itemIndex">the index of the item on the ItemCollectionManager</param>
        public void RequestActivateItem(byte itemIndexInInventory,int[] selectedEntities,Vector3[] positions);
        /// <summary>
        /// Activates an item in the entity's inventory.
        /// </summary>
        /// <param name="index">the index of the item on the ItemCollectionManager</param>
        public void ActivateItemRPC(byte itemIndexInInventory,int[] selectedEntities,Vector3[] positions);
        /// <summary>
        /// Sends an RPC to all clients to activate an item in the entity's inventory.
        /// </summary>
        /// <param name="index">the index of the item on the ItemCollectionManager</param>
        public void SyncActivateItemRPC(byte itemIndexInInventory,int[] selectedEntities,Vector3[] positions,bool[] successes);
        public event GlobalDelegates.ByteIntArrayVector3ArrayDelegate OnActivateItem;
        public event GlobalDelegates.ByteIntArrayVector3ArrayBoolArrayDelegate OnActivateItemFeedback;
        

    }
}


