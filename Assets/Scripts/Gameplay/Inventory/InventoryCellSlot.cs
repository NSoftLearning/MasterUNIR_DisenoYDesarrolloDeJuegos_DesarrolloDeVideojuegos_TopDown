using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCellSlot : MonoBehaviour
{
    [Header("Item View")]
    [SerializeField] private Transform itemImageParent;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemAmountText;

    [Header("Use Button")]
    [SerializeField] private Button useButton;

    private InventorySlot _slot;
    private int _inventoryIndex = -1;

    public Transform GetItemParent()
    {
        return itemImageParent;
    }

    public void Initialize(InventorySlot slot, int inventoryIndex)
    {
        _slot = slot;
        _inventoryIndex = inventoryIndex;

        RefreshView();

        if (useButton != null)
        {
            useButton.onClick.RemoveAllListeners();
            useButton.onClick.AddListener(UseItem);
        }
    }

    private void RefreshView()
    {
        if (_slot == null || _slot.IsEmpty)
        {
            if (itemNameText != null)
                itemNameText.text = "";

            if (itemAmountText != null)
                itemAmountText.text = "";

            if (useButton != null)
                useButton.interactable = false;

            return;
        }

        if (itemNameText != null)
            itemNameText.text = _slot.Item.itemName;

        if (itemAmountText != null)
            itemAmountText.text = _slot.ItemsInSlot.ToString();

        if (useButton != null)
            useButton.interactable = true;
    }

    private void UseItem()
    {
        if (_inventoryIndex < 0)
            return;

        ComponentLocatorService.Components.InventoryManager.UseItemAt(_inventoryIndex);
    }
}