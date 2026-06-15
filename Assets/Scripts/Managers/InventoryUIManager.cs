using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class InventoryUIManager : MonoBehaviour
{
    public List<GameObject> inventorySlotGraphics;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject baseItemPrefab;

    public void AddItem(ItemSO itemData)
    {
        GameObject newSlot = Instantiate(slotPrefab, transform);
        GameObject itemInstance = Instantiate(baseItemPrefab, newSlot.transform);
        itemInstance.GetComponent<Image>().sprite = itemData.ItemIcon;
        inventorySlotGraphics.Add(newSlot);
    }

    public void RemoveItemFromIndex(int index) { 
        GameObject targetItem = inventorySlotGraphics[index];
        if (targetItem != null)
        {
            inventorySlotGraphics.RemoveAt(index);
            Destroy(targetItem);
        }
    }
    public void ClearInventory() { 
        for (int i = 0; i < inventorySlotGraphics.Count; i++)
        {

            Destroy(inventorySlotGraphics[i].gameObject); 

        }
        inventorySlotGraphics.Clear();
    }


    public void InitializeInventoryUI(InventorySO inventoryData)
    {
        List<InventorySlot> inventorySlots = inventoryData.GetInventoryData();
        foreach (InventorySlot slot in inventorySlots) 
        {
            AddItem(slot.Item);
        }
    }

    

}
