using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    [SerializeField] private int _itemsInSlot;
    [SerializeField] private ItemSO _item;

    public ItemSO Item => _item;
    public int ItemsInSlot => _itemsInSlot;
    public bool IsEmpty => _item == null || _itemsInSlot <= 0;

    public void InitializeSlot(ItemSO itemData, int itemCount)
    {
        _item = itemData;
        _itemsInSlot = itemCount;
    }

    public void Clear()
    {
        _item = null;
        _itemsInSlot = 0;
    }

    public void AddAmount(int amount)
    {
        _itemsInSlot += amount;
    }

    public void RemoveAmount(int amount)
    {
        _itemsInSlot -= amount;

        if (_itemsInSlot <= 0)
        {
            Clear();
        }
    }
}