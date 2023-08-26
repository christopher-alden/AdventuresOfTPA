using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] GameObject shopContainer;
    [SerializeField] GameObject parentContainer;
    [SerializeField] GameObject itemsPrefab;
    [SerializeField] List<Item> itemList;
    private Inventory inventory;

    #region Singleton
    private static ShopManager instance;
    public static ShopManager Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        shopContainer.SetActive(false);
        
    }
    #endregion
    
    private void Start()    
    {
        inventory = Inventory.Instance;
        foreach (Item shopItem in itemList)
        {
            GameObject newItemPrefab = Instantiate(itemsPrefab, parentContainer.transform);
            ItemsUI newItemUI = newItemPrefab.GetComponent<ItemsUI>();
            newItemUI.SetShopItem(shopItem);
        }
    }
    public void Show()
    {
        shopContainer.SetActive(true);
    }
    public void Hide()
    {
        shopContainer.SetActive(false);
    }
    
    public void PurchaseItem(Item newItem)
    {
        if (inventory.Money < newItem.Price)
        {
            Debug.Log("Cannot Purchase Item");
            return;
        }
        inventory.Money -= newItem.Price;
        inventory.Add(newItem);

    }
}
