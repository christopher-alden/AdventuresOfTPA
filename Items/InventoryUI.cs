using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryContainer;
    [SerializeField] GameObject parentContainer;
    [SerializeField] GameObject itemsPrefab;
    private Inventory inventory;
    private CursorManager cursorManager;
    private CinemachineController cinemachineController;

    private static InventoryUI instance;

    #region Singleton
    public static InventoryUI Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion
    private void Start()
    {
        inventory = Inventory.Instance;
        cursorManager = CursorManager.Instance;
        cinemachineController = CinemachineController.Instance;
        UpdateUI();
        inventory.onItemChangedCallback += UpdateUI;
        inventoryContainer.SetActive(false);
    }

    public void ToggleInventory(bool inventoryState)
    {
        if (inventoryState)
        {
            cinemachineController.LockCamera();
            cursorManager.EnableCursor();
            inventoryContainer.SetActive(true);
        }
        else
        {
            cinemachineController.UnlockCamera();
            cursorManager.DisableCursor();
            inventoryContainer.SetActive(false);
        }
    }

    public void UpdateUI()
    {
        foreach (Transform child in parentContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in inventory.items)
        {
            GameObject newItemPrefab = Instantiate(itemsPrefab, parentContainer.transform);
            ItemsUI newItemUI = newItemPrefab.GetComponent<ItemsUI>();
            newItemUI.SetInventoryItem(item);
        }
    }
    private void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.onItemChangedCallback -= UpdateUI;
        }
    }

}
