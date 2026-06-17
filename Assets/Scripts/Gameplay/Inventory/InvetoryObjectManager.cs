using UnityEngine;

public class InvetoryObjectManager : MonoBehaviour
{
    [SerializeField] private DraggableItem _item;
    public void DropItem()
    {
        InventoryManager.Instance.RemoveItem(_item.ItemData, 1);
    }

    public void RemoveQuickAccessItem()
    {
        QuickAccessCellLogic quickAccessSlot = GetComponentInParent<QuickAccessCellLogic>();
        if(quickAccessSlot != null)
        {
            InventoryManager.Instance.RemoveItemFromQuickAccess(quickAccessSlot.quickAccessIndex);
        }
    }
}
