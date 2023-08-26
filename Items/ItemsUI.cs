    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class ItemsUI : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI nameText;
        //[SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Button button;
        private ShopManager shopManager;
        private Inventory inventory;
        private Item currItem;

        public void Start()
        {
            shopManager = ShopManager.Instance;
            inventory = Inventory.Instance;
        }
        public void SetShopItem(Item item)
        {
            currItem = item;
            iconImage.sprite = item.Icon;
            nameText.text = item.Name;
            //priceText.text = item.Price.ToString();
            button.onClick.AddListener(ShopOnClick);
        }

        public void SetInventoryItem(Item item)
        {
            currItem = item;
            iconImage.sprite = item.Icon;
            iconImage.enabled = true;
            nameText.text = item.Name;
            //priceText.text = item.Price.ToString();
            button.onClick.AddListener(UseItemOnClick);
        }
        public void ClearInventoryItem()
        {
            currItem = null;
            iconImage.sprite = null;
            iconImage.enabled = false;
            nameText.text = null;
            button.onClick.RemoveAllListeners();
        }

        public void ShopOnClick()
        {
            shopManager.PurchaseItem(currItem);
        }
        public void UseItemOnClick()
        {
            inventory.Remove(currItem);
            ClearInventoryItem();
        }
    }
