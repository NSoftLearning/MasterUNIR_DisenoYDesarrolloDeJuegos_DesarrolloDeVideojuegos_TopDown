using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "Content/Inventory/InventoryInstance")]
public class InventorySO : ScriptableObject
{
    [SerializeField] List<InventorySlot> _inventorySlots;
}
