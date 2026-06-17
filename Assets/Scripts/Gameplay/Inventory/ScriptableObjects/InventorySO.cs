using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "Content/Inventory/InventoryInstance")]
public class InventorySO : ScriptableObject
{
    [Header("Inventory")]
    [SerializeField] private List<InventorySlot> _inventorySlots = new List<InventorySlot>();

    [Header("Inventory Settings")]
    [SerializeField] private bool _allowStacking = false;

    [Header("Quick Access")]
    [SerializeField] private int quickAccessSlotCount = 2;
    [SerializeField] private InventorySlot[] _quickAccessSlots;
    
    public IReadOnlyList<InventorySlot> InventorySlots => _inventorySlots;
    public IReadOnlyList<InventorySlot> QuickAccessSlots => _quickAccessSlots;

    private void OnEnable()
    {
        InitializeQuickAccessSlots();
    }

    private void InitializeQuickAccessSlots()
    {
        if (_quickAccessSlots == null || _quickAccessSlots.Length != quickAccessSlotCount)
        {
            _quickAccessSlots = new InventorySlot[quickAccessSlotCount];
        }

        for (int i = 0; i < _quickAccessSlots.Length; i++)
        {
            if (_quickAccessSlots[i] == null)
            {
                _quickAccessSlots[i] = new InventorySlot();
            }
        }
    }

    public void ClearInventory()
    {
        _inventorySlots.Clear();

        InitializeQuickAccessSlots();

        for (int i = 0; i < _quickAccessSlots.Length; i++)
        {
            _quickAccessSlots[i].Clear();
        }
    }

    public void AddItem(ItemSO itemData)
    {
        if (itemData == null) return;

        if (_allowStacking)
        {
            InventorySlot existingSlot = FindSlotWithItem(itemData);

            if (existingSlot != null)
            {
                existingSlot.AddAmount(1);
                UpdateQuickAccessSlotsWithItem(itemData, existingSlot.ItemsInSlot);
                return;
            }
        }

        InventorySlot itemSlot = new InventorySlot();
        itemSlot.InitializeSlot(itemData, 1);
        _inventorySlots.Add(itemSlot);
    }

    public bool RemoveItem(ItemSO itemData, int amount = 1)
    {
        if (itemData == null || amount <= 0)
        {
            return false;
        }

        InventorySlot slot = FindSlotWithItem(itemData);

        if (slot == null)
        {
            Debug.LogWarning($"Item {itemData.itemName} is not in the inventory.");
            return false;
        }

        slot.RemoveAmount(amount);

        if (slot.IsEmpty)
        {
            _inventorySlots.Remove(slot);
            ClearQuickAccessSlotsWithItem(itemData);
        }
        else
        {
            UpdateQuickAccessSlotsWithItem(itemData, slot.ItemsInSlot);
        }

        return true;
    }

    public void AssignItemToQuickAccess(int inventoryIndex, int quickAccessIndex)
    {
        InitializeQuickAccessSlots();

        if (quickAccessIndex < 0 || quickAccessIndex >= _quickAccessSlots.Length)
        {
            Debug.LogWarning("Invalid quick access index.");
            return;
        }

        if (inventoryIndex < 0 || inventoryIndex >= _inventorySlots.Count)
        {
            Debug.LogWarning("Invalid inventory index.");
            _quickAccessSlots[quickAccessIndex].Clear();
            return;
        }

        InventorySlot inventorySlot = _inventorySlots[inventoryIndex];

        if (inventorySlot == null || inventorySlot.IsEmpty)
        {
            Debug.LogWarning("Cannot assign an empty inventory slot.");
            return;
        }

        _quickAccessSlots[quickAccessIndex].InitializeSlot(
            inventorySlot.Item,
            inventorySlot.ItemsInSlot
        );
    }

    public void RemoveItemFromQuickAccess(int quickAccessIndex) { 
        InitializeQuickAccessSlots();
        if (quickAccessIndex < 0 || quickAccessIndex >= _quickAccessSlots.Length)
        {
            Debug.LogWarning("Invalid quick access index.");
            return;
        }
        _quickAccessSlots[quickAccessIndex].Clear();
    }

    public ItemSO GetQuickAccessItem(int quickAccessIndex)
    {
        InitializeQuickAccessSlots();

        if (quickAccessIndex < 0 || quickAccessIndex >= _quickAccessSlots.Length)
        {
            return null;
        }

        if (_quickAccessSlots[quickAccessIndex].IsEmpty)
        {
            return null;
        }

        return _quickAccessSlots[quickAccessIndex].Item;
    }

    public void ClearQuickAccessSlot(int quickAccessIndex)
    {
        InitializeQuickAccessSlots();

        if (quickAccessIndex < 0 || quickAccessIndex >= _quickAccessSlots.Length)
        {
            return;
        }

        _quickAccessSlots[quickAccessIndex].Clear();
    }

    private InventorySlot FindSlotWithItem(ItemSO itemData)
    {
        foreach (InventorySlot slot in _inventorySlots)
        {
            if (slot.Item == itemData)
            {
                return slot;
            }
        }

        return null;
    }

    private void ClearQuickAccessSlotsWithItem(ItemSO itemData)
    {
        InitializeQuickAccessSlots();

        for (int i = 0; i < _quickAccessSlots.Length; i++)
        {
            if (_quickAccessSlots[i].Item == itemData)
            {
                _quickAccessSlots[i].Clear();
            }
        }
    }

    private void UpdateQuickAccessSlotsWithItem(ItemSO itemData, int newAmount)
    {
        InitializeQuickAccessSlots();

        for (int i = 0; i < _quickAccessSlots.Length; i++)
        {
            if (_quickAccessSlots[i].Item == itemData)
            {
                _quickAccessSlots[i].InitializeSlot(itemData, newAmount);
            }
        }
    }
    public bool RemoveItemAt(int inventoryIndex)
    {
        if (inventoryIndex < 0 || inventoryIndex >= _inventorySlots.Count)
        {
            Debug.LogWarning("Invalid inventory index.");
            return false;
        }

        InventorySlot slot = _inventorySlots[inventoryIndex];

        if (slot == null || slot.IsEmpty)
        {
            Debug.LogWarning("Cannot remove an empty inventory slot.");
            return false;
        }

        ItemSO removedItem = slot.Item;

        _inventorySlots.RemoveAt(inventoryIndex);

        ClearQuickAccessSlotsWithItem(removedItem);

        return true;
    }

    public List<InventorySlot> GetInventoryData() 
    { 
        return _inventorySlots; 
    }
}