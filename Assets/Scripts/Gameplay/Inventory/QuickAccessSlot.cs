using System;
using UnityEngine;

[Serializable]
public class QuickAccessSlot
{
    [SerializeField] private int _inventoryIndex = -1;

    public int InventoryIndex => _inventoryIndex;
    public bool IsEmpty => _inventoryIndex < 0;

    public void Assign(int inventoryIndex)
    {
        _inventoryIndex = inventoryIndex;
    }

    public void Clear()
    {
        _inventoryIndex = -1;
    }
}