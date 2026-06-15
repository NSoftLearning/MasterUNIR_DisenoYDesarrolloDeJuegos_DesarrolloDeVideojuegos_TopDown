using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    [SerializeField] int _itemsInSlot;
    [SerializeField] ItemSO _item;


    public void InitializeSlot(ItemSO itemData, int itemCount)
    {
        _item = itemData;
        _itemsInSlot = itemCount;
    }
}
