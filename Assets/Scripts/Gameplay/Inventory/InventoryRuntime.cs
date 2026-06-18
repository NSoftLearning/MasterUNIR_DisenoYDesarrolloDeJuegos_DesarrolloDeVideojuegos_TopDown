using System.Collections.Generic;
using UnityEngine;

public class InventoryRuntime
{
    private readonly List<InventorySlot> _inventorySlots = new List<InventorySlot>();
    private readonly QuickAccessSlot[] _quickAccessSlots;
    private readonly bool _allowStacking;

    public IReadOnlyList<InventorySlot> InventorySlots => _inventorySlots;
    public IReadOnlyList<QuickAccessSlot> QuickAccessSlots => _quickAccessSlots;

    public InventoryRuntime(InventorySO inventorySO)
    {
        if (inventorySO == null)
        {
            Debug.LogError("InventoryRuntime cannot be created from a null InventorySO.");
            _quickAccessSlots = new QuickAccessSlot[0];
            return;
        }

        _allowStacking = inventorySO.AllowStacking;

        _quickAccessSlots = new QuickAccessSlot[inventorySO.QuickAccessSlotCount];

        for (int i = 0; i < _quickAccessSlots.Length; i++)
        {
            _quickAccessSlots[i] = new QuickAccessSlot();
        }

        LoadInventorySlots(inventorySO.SavedSlots);
        LoadQuickAccessSlots(inventorySO.SavedQuickAccessIndexes);
    }

    private void LoadInventorySlots(IReadOnlyList<InventorySlot> savedSlots)
    {
        _inventorySlots.Clear();

        if (savedSlots == null)
            return;

        for (int i = 0; i < savedSlots.Count; i++)
        {
            InventorySlot savedSlot = savedSlots[i];

            if (savedSlot == null || savedSlot.IsEmpty)
                continue;

            InventorySlot copy = new InventorySlot();
            copy.InitializeSlot(savedSlot.Item, savedSlot.ItemsInSlot);

            _inventorySlots.Add(copy);
        }
    }

    private void LoadQuickAccessSlots(IReadOnlyList<int> savedQuickAccessIndexes)
    {
        if (savedQuickAccessIndexes == null)
            return;

        int count = Mathf.Min(savedQuickAccessIndexes.Count, _quickAccessSlots.Length);

        for (int i = 0; i < count; i++)
        {
            int inventoryIndex = savedQuickAccessIndexes[i];

            if (inventoryIndex < 0 || inventoryIndex >= _inventorySlots.Count)
            {
                _quickAccessSlots[i].Clear();
            }
            else
            {
                _quickAccessSlots[i].Assign(inventoryIndex);
            }
        }
    }

    public void ClearInventory()
    {
        _inventorySlots.Clear();

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

    public bool UseItemAt(int inventoryIndex, GameObject user)
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

        ItemSO item = slot.Item;

        if (item == null)
            return false;

        bool usedSuccessfully = item.UseItem(user);

        if (!usedSuccessfully)
            return false;

        if (item.ConsumeOnUse)
        {
            slot.RemoveAmount(1);

            if (slot.IsEmpty)
            {
                _inventorySlots.RemoveAt(inventoryIndex);

                ClearQuickAccessSlotsWithInventoryIndex(inventoryIndex);
                FixQuickAccessIndexesAfterInventoryRemove(inventoryIndex);
            }
        }

        return true;
    }

    public bool RemoveItem(ItemSO itemData, int amount = 1)
    {
        if (itemData == null || amount <= 0)
            return false;

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
        if (quickAccessIndex < 0 || quickAccessIndex >= _quickAccessSlots.Length)
        {
            Debug.LogWarning("Invalid quick access index.");
            return;
        }

        _quickAccessSlots[quickAccessIndex].Clear();
    }

    public InventorySlot GetQuickAccessInventorySlot(int quickAccessIndex)
    {
        if (quickAccessIndex < 0 || quickAccessIndex >= _quickAccessSlots.Length)
            return null;

        QuickAccessSlot quickSlot = _quickAccessSlots[quickAccessIndex];

        if (quickSlot == null || quickSlot.IsEmpty)
            return null;

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

    public ItemSO GetQuickAccessItem(int quickAccessIndex)
    {
        InventorySlot slot = GetQuickAccessInventorySlot(quickAccessIndex);

        if (slot == null || slot.IsEmpty)
            return null;

        return slot.Item;
    }

    public InventorySlot GetInventorySlot(int inventoryIndex)
    {
        if (inventoryIndex < 0 || inventoryIndex >= _inventorySlots.Count)
            return null;

        return _inventorySlots[inventoryIndex];
    }

    private InventorySlot FindSlotWithItem(ItemSO itemData)
    {
        for (int i = 0; i < _inventorySlots.Count; i++)
        {
            InventorySlot slot = _inventorySlots[i];

            if (slot != null && slot.Item == itemData)
            {
                return slot;
            }
        }

        return null;
    }

    private void ClearQuickAccessSlotsWithInventoryIndex(int inventoryIndex)
    {
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