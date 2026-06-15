using UnityEngine;
using UnityEngine.EventSystems;
public class InventoryCellLogic : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;
        DraggableItem item = droppedItem.GetComponent<DraggableItem>();
        item.parentAfterDrag = transform;
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
