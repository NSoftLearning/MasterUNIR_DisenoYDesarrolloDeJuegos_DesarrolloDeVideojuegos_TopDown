using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "Content/Inventory/InventoryInstance")]
public class InventorySO : ScriptableObject
{
    [SerializeField] List<InventorySlot> _inventorySlots;



    public void ClearInventory()
    {
        _inventorySlots.Clear();
    }

    public void AddItem(ItemSO itemData)
    {
        InventorySlot itemSlot = new InventorySlot();
        itemSlot.InitializeSlot(itemData, 1);
        _inventorySlots.Add(itemSlot);

    }
}
