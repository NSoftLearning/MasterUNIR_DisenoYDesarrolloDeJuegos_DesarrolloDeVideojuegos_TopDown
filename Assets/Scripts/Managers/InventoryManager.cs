using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] InventorySO currentInventory;
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
        }
    }

    private void Update()
    {
        if (clearInventory)
        {
            clearInventory = false;
            ClearInventory();
        }

        if (updateQuickAccess) {
            AssignItemToQuickAccess(inventoryIndexToSetInFirstQuickAccess, 0);
            AssignItemToQuickAccess(inventoryIndexToSetInSecondQuickAccess, 1);
        }
    }

    public void AddItem(ItemSO itemData) { 
        currentInventory.AddItem(itemData);
    
    }

    public void ClearInventory()
    {
        currentInventory.ClearInventory();
    }
    public void AssignItemToQuickAccess(int inventoryIndex, int quickAccessIndex)
    {
        currentInventory.AssignItemToQuickAccess(inventoryIndex, quickAccessIndex);
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
        return currentInventory.RemoveItem(itemData, amount);
    }
    public bool RemoveItemAt(int inventoryIndex)
    {
        return currentInventory.RemoveItemAt(inventoryIndex);
    }
}
