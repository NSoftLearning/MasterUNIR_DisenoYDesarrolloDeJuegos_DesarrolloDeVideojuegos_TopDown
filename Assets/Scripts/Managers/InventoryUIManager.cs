using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

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
            GameObject itemInstance = Instantiate(baseItemPrefab, newSlot.transform);

            DraggableItem draggableItem = itemInstance.GetComponent<DraggableItem>();
            draggableItem.Initialize(slot.Item, i);

            inventorySlotGraphics.Add(newSlot);
        }
    }

    private void DrawQuickAccess(InventorySO inventoryData)
    {
        IReadOnlyList<InventorySlot> quickSlots = inventoryData.QuickAccessSlots;

        for (int i = 0; i < quickAccessSlots.Count; i++)
        {
            quickAccessItemGraphics.Add(null);

            if (i >= quickSlots.Count)
                continue;

            InventorySlot slot = quickSlots[i];

            if (slot == null || slot.IsEmpty)
                continue;

            GameObject itemGraphic = Instantiate(quickAccessItemPrefab, quickAccessSlots[i]);

            Image image = itemGraphic.GetComponent<Image>();
            image.sprite = slot.Item.ItemIcon;

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