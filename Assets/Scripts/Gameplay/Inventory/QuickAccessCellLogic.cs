using UnityEngine;
using UnityEngine.EventSystems;

public class QuickAccessCellLogic : MonoBehaviour, IDropHandler
{
    [SerializeField] public int quickAccessIndex;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;

        if (droppedItem == null)
            return;

        DraggableItem draggableItem = droppedItem.GetComponent<DraggableItem>();

        if (draggableItem == null)
            return;

        draggableItem.MarkAsDroppedSuccessfully();

        InventoryManager.Instance.AssignItemToQuickAccess(
            draggableItem.InventoryIndex,
            quickAccessIndex
        );
    }
}