using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InventoryUIManager inventoryUI;
    [SerializeField] private InventorySO currentInventory;

    public static InventoryManager Instance;

    [Header("Debug Settings")]
    public bool clearInventory = false;
    public bool updateQuickAccess = false;
    public int inventoryIndexToSetInFirstQuickAccess = 0;
    public int inventoryIndexToSetInSecondQuickAccess = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        RefreshUI();
    }

    private void Update()
    {
        if (clearInventory)
        {
            clearInventory = false;
            ClearInventory();
        }

        if (updateQuickAccess)
        {
            updateQuickAccess = false;

            AssignItemToQuickAccess(inventoryIndexToSetInFirstQuickAccess, 0);
            AssignItemToQuickAccess(inventoryIndexToSetInSecondQuickAccess, 1);
        }
    }

    public void AddItem(ItemSO itemData)
    {
        currentInventory.AddItem(itemData);
        RefreshUI();
    }

    public void ClearInventory()
    {
        currentInventory.ClearInventory();
        RefreshUI();
    }

    public void AssignItemToQuickAccess(int inventoryIndex, int quickAccessIndex)
    {
        currentInventory.AssignItemToQuickAccess(inventoryIndex, quickAccessIndex);
        RefreshUI();
    }
    public void RemoveItemFromQuickAccess(int quickAccessIndex)
    {
        currentInventory.RemoveItemFromQuickAccess(quickAccessIndex);
        RefreshUI();
    }

    public void UseQuickAccessItem(int quickAccessIndex)
    {
        ItemSO item = currentInventory.GetQuickAccessItem(quickAccessIndex);

        if (item == null)
        {
            Debug.Log($"Quick access slot {quickAccessIndex + 1} is empty.");
            return;
        }

        Debug.Log($"Using item: {item.itemName}");
        item.UseItem();
    }

    public InventorySO GetInventory()
    {
        return currentInventory;
    }

    public bool RemoveItem(ItemSO itemData, int amount = 1)
    {
        bool removed = currentInventory.RemoveItem(itemData, amount);

        if (removed)
        {
            RefreshUI();
        }

        return removed;
    }

    public bool RemoveItemAt(int inventoryIndex)
    {
        bool removed = currentInventory.RemoveItemAt(inventoryIndex);

        if (removed)
        {
            RefreshUI();
        }

        return removed;
    }

    public void RefreshUI()
    {
        if (inventoryUI != null && currentInventory != null)
        {
            inventoryUI.RefreshInventoryUI(currentInventory);
        }
    }
}