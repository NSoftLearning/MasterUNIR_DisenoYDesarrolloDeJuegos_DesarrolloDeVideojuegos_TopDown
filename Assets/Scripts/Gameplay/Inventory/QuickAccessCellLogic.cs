using UnityEngine;
using UnityEngine.EventSystems;
public class QuickAccessCellLogic : MonoBehaviour, IDropHandler
{
    [SerializeField] private GameObject quickAccessGraphicPrefab;
    [SerializeField] private GameObject inventaryItemReference;
    private GameObject itemGraphic;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;
        if (droppedItem == null)
            return;
        if(inventaryItemReference != null)
        {
            Destroy(inventaryItemReference);
        }
        if(itemGraphic != null)
            Destroy(itemGraphic);
        inventaryItemReference = droppedItem;
        itemGraphic = Instantiate(quickAccessGraphicPrefab, transform);
        //...
        

    }
}
