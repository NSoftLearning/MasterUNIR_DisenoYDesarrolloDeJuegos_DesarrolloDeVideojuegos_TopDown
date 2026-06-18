using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryUIManager : MonoBehaviour
{
    [Header("Inventory Panel Slide")]
    [SerializeField] private RectTransform slidingPanel;
    [SerializeField] private GameObject openButton;
    [SerializeField] private GameObject closeButton;

    [SerializeField] private Vector2 openedPosition;
    [SerializeField] private Vector2 closedPosition;
    [SerializeField] private float slideDuration = 0.25f;
    [SerializeField] private bool startClosed = true;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotsParent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject baseItemPrefab;

    [Header("Quick Access UI")]
    [SerializeField] private List<Transform> quickAccessSlots;
    [SerializeField] private GameObject quickAccessItemPrefab;

    private readonly List<GameObject> inventorySlotGraphics = new List<GameObject>();
    private readonly List<GameObject> quickAccessItemGraphics = new List<GameObject>();

    private bool isOpen;
    private Coroutine slideCoroutine;

    private void Start()
    {
        if (startClosed)
        {
            SetClosedInstant();
        }
        else
        {
            SetOpenInstant();
        }
    }

    public void OpenInventory()
    {
        if (slidingPanel == null)
            return;

        StartSlide(openedPosition, true);
    }

    public void CloseInventory()
    {
        if (slidingPanel == null)
            return;

        StartSlide(closedPosition, false);
    }

    public void ToggleInventory()
    {
        if (isOpen)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }

    private void SetOpenInstant()
    {
        if (slidingPanel != null)
        {
            slidingPanel.anchoredPosition = openedPosition;
        }

        isOpen = true;
        RefreshPanelButtons();
    }

    private void SetClosedInstant()
    {
        if (slidingPanel != null)
        {
            slidingPanel.anchoredPosition = closedPosition;
        }

        isOpen = false;
        RefreshPanelButtons();
    }

    private void StartSlide(Vector2 targetPosition, bool targetOpenState)
    {
        if (slideCoroutine != null)
        {
            StopCoroutine(slideCoroutine);
        }

        slideCoroutine = StartCoroutine(SlidePanel(targetPosition, targetOpenState));
    }

    private IEnumerator SlidePanel(Vector2 targetPosition, bool targetOpenState)
    {
        SetButtonsVisible(false, false);

        Vector2 startPosition = slidingPanel.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / slideDuration;
            t = Mathf.Clamp01(t);
            t = Mathf.SmoothStep(0f, 1f, t);

            slidingPanel.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        slidingPanel.anchoredPosition = targetPosition;

        isOpen = targetOpenState;
        RefreshPanelButtons();

        slideCoroutine = null;
    }

    private void RefreshPanelButtons()
    {
        SetButtonsVisible(!isOpen, isOpen);
    }

    private void SetButtonsVisible(bool showOpenButton, bool showCloseButton)
    {
        if (openButton != null)
        {
            openButton.SetActive(showOpenButton);
        }

        if (closeButton != null)
        {
            closeButton.SetActive(showCloseButton);
        }
    }

    public void InitializeInventoryUI(InventoryRuntime inventoryData)
    {
        RefreshInventoryUI(inventoryData);
    }

    public void RefreshInventoryUI(InventoryRuntime inventoryData)
    {
        ClearInventoryUI();
        ClearQuickAccessUI();

        if (inventoryData == null)
            return;

        DrawInventory(inventoryData);
        DrawQuickAccess(inventoryData);
    }

    private void DrawInventory(InventoryRuntime inventoryData)
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

    private void DrawQuickAccess(InventoryRuntime inventoryData)
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