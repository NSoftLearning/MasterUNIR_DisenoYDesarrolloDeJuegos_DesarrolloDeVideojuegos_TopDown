using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    [SerializeField] int _itemsInSlot;
    [SerializeField] ItemSO _item;
}
