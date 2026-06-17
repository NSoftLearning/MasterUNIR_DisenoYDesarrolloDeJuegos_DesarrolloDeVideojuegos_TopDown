using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "Content/Inventory/InventoryInstance")]
public class InventorySO : ScriptableObject
{
    [Header("Inventory")]
    [SerializeField] private List<InventorySlot> _inventorySlots = new List<InventorySlot>();

    [Header("Inventory Settings")]
    [SerializeField] private bool _allowStacking = true;

    [Header("Quick Access")]
    [SerializeField] private int quickAccessSlotCount = 2;
    [SerializeField] private QuickAccessSlot[] _quickAccessSlots;

    public IReadOnlyList<InventorySlot> InventorySlots => _inventorySlots;
    public IReadOnlyList<QuickAccessSlot> QuickAccessSlots => _quickAccessSlots;

    private void OnEnable()
    {
        InitializeQuickAccessSlots();
    }

    private void InitializeQuickAccessSlots()
    {
        if (_quickAccessSlots == null || _quickAccessSlots.Length != quickAccessSlotCount)
        {
            _quickAccessSlots = new QuickAccessSlot[quickAccessSlotCount];
        }

        for (int i = 0; i < _quickAccessSlots.Length; i++)
        {
            if (_quickAccessSlots[i] == null)
            {
                _quickAccessSlots[i] = new QuickAccessSlot();
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
        if (itemData == null)
            return;

        if (_allowStacking)
        {
            InventorySlot existingSlot = FindSlotWithItem(itemData);

            if (existingSlot != null)
            {
                existingSlot.AddAmount(1);
                return;
            }
        }

        InventorySlot itemSlot = new InventorySlot();
        itemSlot.InitializeSlot(itemData, 1);
        _inventorySlots.Add(itemSlot);
    }

    public bool UseItemAt(int inventoryIndex)
    {
        if (inventoryIndex < 0 || inventoryIndex >= _inventorySlots.Count)
        {
            Debug.LogWarning("Invalid inventory index.");
            return false;
        }

        InventorySlot slot = _inventorySlots[inventoryIndex];

        if (slot == null || slot.IsEmpty)
        {
            Debug.LogWarning("Cannot use an empty inventory slot.");
            return false;
        }

        slot.Item.UseItem();
        slot.RemoveAmount(1);

        if (slot.IsEmpty)
        {
            _inventorySlots.RemoveAt(inventoryIndex);

            ClearQuickAccessSlotsWithInventoryIndex(inventoryIndex);
            FixQuickAccessIndexesAfterInventoryRemove(inventoryIndex);
        }

        return true;
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

        int removedIndex = _inventorySlots.IndexOf(slot);

        slot.RemoveAmount(amount);

        if (slot.IsEmpty)
        {
            _inventorySlots.RemoveAt(removedIndex);

            ClearQuickAccessSlotsWithInventoryIndex(removedIndex);
            FixQuickAccessIndexesAfterInventoryRemove(removedIndex);
        }

        return true;
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

        _inventorySlots.RemoveAt(inventoryIndex);

        ClearQuickAccessSlotsWithInventoryIndex(inventoryIndex);
        FixQuickAccessIndexesAfterInventoryRemove(inventoryIndex);

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

        ClearQuickAccessSlotsWithInventoryIndex(inventoryIndex);

        _quickAccessSlots[quickAccessIndex].Assign(inventoryIndex);
    }

    public void RemoveItemFromQuickAccess(int quickAccessIndex)
    {
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
        InventorySlot slot = GetQuickAccessInventorySlot(quickAccessIndex);

        if (slot == null || slot.IsEmpty)
        {
            return null;
        }

        return slot.Item;
    }

    public InventorySlot GetQuickAccessInventorySlot(int quickAccessIndex)
    {
        InitializeQuickAccessSlots();

        if (quickAccessIndex < 0 || quickAccessIndex >= _quickAccessSlots.Length)
        {
            return null;
        }

        QuickAccessSlot quickSlot = _quickAccessSlots[quickAccessIndex];

        if (quickSlot == null || quickSlot.IsEmpty)
        {
            return null;
        }

        int inventoryIndex = quickSlot.InventoryIndex;

        if (inventoryIndex < 0 || inventoryIndex >= _inventorySlots.Count)
        {
            quickSlot.Clear();
            return null;
        }

        InventorySlot inventorySlot = _inventorySlots[inventoryIndex];

        if (inventorySlot == null || inventorySlot.IsEmpty)
        {
            quickSlot.Clear();
            return null;
        }

        return inventorySlot;
    }

    public InventorySlot GetInventorySlot(int inventoryIndex)
    {
        if (inventoryIndex < 0 || inventoryIndex >= _inventorySlots.Count)
        {
            return null;
        }

        return _inventorySlots[inventoryIndex];
    }

    public List<InventorySlot> GetInventoryData()
    {
        return _inventorySlots;
    }

    private InventorySlot FindSlotWithItem(ItemSO itemData)
    {
        foreach (InventorySlot slot in _inventorySlots)
        {
            if (slot != null && slot.Item == itemData)
            {
                return slot;
            }
        }

        return null;
    }

    private void ClearQuickAccessSlotsWithInventoryIndex(int inventoryIndex)
    {
        InitializeQuickAccessSlots();

        for (int i = 0; i < _quickAccessSlots.Length; i++)
        {
            QuickAccessSlot quickSlot = _quickAccessSlots[i];

            if (quickSlot == null || quickSlot.IsEmpty)
                continue;

            if (quickSlot.InventoryIndex == inventoryIndex)
            {
                quickSlot.Clear();
            }
        }
    }

    private void FixQuickAccessIndexesAfterInventoryRemove(int removedInventoryIndex)
    {
        InitializeQuickAccessSlots();

        for (int i = 0; i < _quickAccessSlots.Length; i++)
        {
            QuickAccessSlot quickSlot = _quickAccessSlots[i];

            if (quickSlot == null || quickSlot.IsEmpty)
                continue;

            int currentIndex = quickSlot.InventoryIndex;

            if (currentIndex > removedInventoryIndex)
            {
                quickSlot.Assign(currentIndex - 1);
            }
        }
    }
}