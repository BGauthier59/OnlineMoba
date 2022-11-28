using System.Collections;
using System.Collections.Generic;
using Entities.Inventory;
using GameStates;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    [SerializeField] private List<StockPanel> ShopItemImagesUI;

    [System.Serializable]
    public class StockPanel
    {
        public Image slotImage;
        public Button buttonShop;
    }

    private IEnumerator InitUIShop()
    {
        yield return new WaitForSeconds(0.5f);
        for (byte a = 0; a < ItemCollectionManager.Instance.allItemSOs.Count; a++)
        {
            ShopItemImagesUI[a].slotImage.sprite = ItemCollectionManager.Instance.allItemSOs[a].sprite;
            var indexOfItemToAdd = a;
            ShopItemImagesUI[a].buttonShop.onClick.AddListener(() => GameStateMachine.Instance.GetPlayerChampion().GetComponent<IInventoryable>().RequestAddItem(indexOfItemToAdd));
        }
    }
    
    private void Start()
    {
        StartCoroutine(InitUIShop());
    }
}
