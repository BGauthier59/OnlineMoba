using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Champion;
using Entities.Inventory;
using GameStates;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public partial class UIManager
{
    [SerializeField] private List<InventoryPanel> inventoriesPanel = new List<InventoryPanel>();
    [SerializeField] private List<LocalInventory.LocalInventorySlots> slots = new List<LocalInventory.LocalInventorySlots>();
    private LocalInventory localInventory;
    [SerializeField] private RectTransform InventoryCanvas;
    [SerializeField] private RectTransform ShopCanvas;

    private Dictionary<int, InventoryPanel> inventoryPanelsDict = new Dictionary<int, InventoryPanel>();

    [System.Serializable]
    public class InventoryPanel
    {
        public TextMeshProUGUI playerNameText;
        public List<Image> slotImages;
        public Enums.Team team;
        public IInventoryable inventory;
        [HideInInspector] public bool available = true;

        public void LinkWithInventory(IInventoryable inv)
        {
            inventory = inv;
            inventory.OnAddItemFeedback += UpdateInventory;
            inventory.OnRemoveItemFeedback += UpdateInventory;
        }
        
        private void UpdateInventory(byte _)
        {
            var items = inventory.GetItems();
            for (var i = 0; i < slotImages.Count; i++)
            {
                slotImages[i].sprite = i>=items.Length ? null : items[i].AssociatedItemSO().sprite;
            }
        }
    }
    
    public class LocalInventory
    {
        public IInventoryable inventory;
        
        public List<LocalInventorySlots> slots = new List<LocalInventorySlots>();
        
        [System.Serializable]
        public class LocalInventorySlots
        {
            public Image slotImages;
            public Button slotButton;
        }
        
        public void LinkWithInventory(IInventoryable inv)
        {
            inventory = inv;
            inventory.OnAddItemFeedback += UpdateLocalInventory;
            inventory.OnRemoveItemFeedback += UpdateLocalInventory;
        }

        private void UpdateLocalInventory(byte _)
        {
            var items = inventory.GetItems();
            for (byte i = 0; i < slots.Count; i++)
            {
                
                slots[i].slotImages.sprite = i>=items.Length ? null : items[i].AssociatedItemSO().sprite;
            
                slots[i].slotButton.onClick.RemoveAllListeners();
            
                if (items.Length <= i) continue;
                
                var indexInInventory = i;
                slots[i].slotButton.onClick.AddListener(() => inventory.RequestRemoveItem(indexInInventory));
            }
        }
    }
    
    public void ShowHideInventory(bool show)
    {
        InventoryCanvas.gameObject.SetActive(show);
    }

    public void ShowHideShop()
    {
        ShopCanvas.gameObject.SetActive(!ShopCanvas.gameObject.activeSelf);
    }
    
    public void AssignInventory(int actorNumber)
    {
        var playerTeam = GameStateMachine.Instance.GetPlayerTeam(actorNumber);
        var champion = GameStateMachine.Instance.GetPlayerChampion(actorNumber).GetComponent<IInventoryable>();
        foreach (var panel in inventoriesPanel)
        {
            if (panel.team != playerTeam || !panel.available) continue;
            panel.available = false;
            inventoryPanelsDict.Add(actorNumber, panel);
            panel.playerNameText.text = $"J{actorNumber}";
            panel.LinkWithInventory(champion);
            break;
        }

        if ((Champion)champion == GameStateMachine.Instance.GetPlayerChampion())
        {
            localInventory = new LocalInventory
            {
                slots = slots
            };
            localInventory.LinkWithInventory(champion);
        }
    }
}