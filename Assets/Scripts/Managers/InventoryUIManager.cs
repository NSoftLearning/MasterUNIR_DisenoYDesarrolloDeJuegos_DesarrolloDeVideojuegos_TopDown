using UnityEngine;
using System.Collections.Generic;

public class InventoryUIManager : MonoBehaviour
{
    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotsParent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject baseItemPrefab;

    [Header("Quick Access UI")]
    [SerializeField] private List<Transform> quickAccessSlots;
    [SerializeField] private GameObject quickAccessItemPrefab;

    private readonly List<GameObject> inventorySlotGraphics = new List<GameObject>();
    private readonly List<GameObject> quickAccessItemGraphics = new List<GameObject>();

    public void InitializeInventoryUI(InventorySO inventoryData)
    {
        RefreshInventoryUI(inventoryData);
    }

    public void RefreshInventoryUI(InventorySO inventoryData)
    {
        ClearInventoryUI();
        ClearQuickAccessUI();

        if (inventoryData == null)
            return;

        DrawInventory(inventoryData);
        DrawQuickAccess(inventoryData);
    }

    private void DrawInventory(InventorySO inventoryData)
    {
        IReadOnlyList<InventorySlot> inventorySlots = inventoryData.InventorySlots;

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventorySlot slot = inventorySlots[i];

            if (slot == null || slot.IsEmpty)
                continue;

            GameObject newSlot = Instantiate(slotPrefab, inventorySlotsParent);

            InventoryCellSlot inventoryCellSlot = newSlot.GetComponent<InventoryCellSlot>();

            if (inventoryCellSlot != null)
            {
                inventoryCellSlot.Initialize(slot, i);
            }

            GameObject itemInstance = Instantiate(baseItemPrefab);

            if (inventoryCellSlot != null && inventoryCellSlot.GetItemParent() != null)
            {
                itemInstance.transform.SetParent(inventoryCellSlot.GetItemParent(), false);
            }
            else
            {
                itemInstance.transform.SetParent(newSlot.transform, false);
            }

            DraggableItem draggableItem = itemInstance.GetComponent<DraggableItem>();

            if (draggableItem != null)
            {
                draggableItem.Initialize(slot.Item, i);
            }

            inventorySlotGraphics.Add(newSlot);
        }
    }

    private void DrawQuickAccess(InventorySO inventoryData)
    {
        IReadOnlyList<QuickAccessSlot> quickSlots = inventoryData.QuickAccessSlots;
        IReadOnlyList<InventorySlot> inventorySlots = inventoryData.InventorySlots;

        for (int i = 0; i < quickAccessSlots.Count; i++)
        {
            quickAccessItemGraphics.Add(null);

            if (i >= quickSlots.Count)
                continue;

            QuickAccessSlot quickSlot = quickSlots[i];

            if (quickSlot == null || quickSlot.IsEmpty)
                continue;

            int inventoryIndex = quickSlot.InventoryIndex;

            if (inventoryIndex < 0 || inventoryIndex >= inventorySlots.Count)
                continue;

            InventorySlot inventorySlot = inventorySlots[inventoryIndex];

            if (inventorySlot == null || inventorySlot.IsEmpty)
                continue;

            GameObject itemGraphic = Instantiate(quickAccessItemPrefab, quickAccessSlots[i]);

            DraggableItem draggableItem = itemGraphic.GetComponent<DraggableItem>();

            if (draggableItem != null)
            {
                draggableItem.Initialize(inventorySlot.Item, inventoryIndex);
            }

            quickAccessItemGraphics[i] = itemGraphic;
        }
    }

    private void ClearInventoryUI()
    {
        for (int i = 0; i < inventorySlotGraphics.Count; i++)
        {
            if (inventorySlotGraphics[i] != null)
            {
                Destroy(inventorySlotGraphics[i]);
            }
        }

        inventorySlotGraphics.Clear();
    }

    private void ClearQuickAccessUI()
    {
        for (int i = 0; i < quickAccessItemGraphics.Count; i++)
        {
            if (quickAccessItemGraphics[i] != null)
            {
                Destroy(quickAccessItemGraphics[i]);
            }
        }

        quickAccessItemGraphics.Clear();
    }
}