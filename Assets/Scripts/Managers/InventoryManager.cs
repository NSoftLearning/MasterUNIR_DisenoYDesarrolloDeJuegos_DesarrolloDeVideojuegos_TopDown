#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InventoryUIManager inventoryUI;
    [SerializeField] private InventorySO inventorySO;
    [SerializeField] private GameObject itemUser;

    private InventoryRuntime currentInventory;
    public event Action OnBasicClick;
    public event Action<bool> OnUse;
    public event Action<bool> OnMove;




    [Header("Debug Settings")]
    public bool clearInventory = false;
    public bool updateQuickAccess = false;
    public bool saveRuntimeToInventorySO = false;

    public int inventoryIndexToSetInFirstQuickAccess = 0;
    public int inventoryIndexToSetInSecondQuickAccess = 1;

    private void Awake()
    {
  
        LoadInventoryFromSO();
        RefreshUI();
       
    }
    public void SwitchInventorySO(InventorySO inventory)
    {
        if (inventory == null)
            return;
        inventorySO = inventory;
        LoadInventoryFromSO();
        RefreshUI();
    }
    private void Update()
    {
        if (clearInventory)
        {
            clearInventory = false;
            ClearInventory();
        }

        if (updateQuickAccess)
        {
            updateQuickAccess = false;

            AssignItemToQuickAccess(inventoryIndexToSetInFirstQuickAccess, 0);
            AssignItemToQuickAccess(inventoryIndexToSetInSecondQuickAccess, 1);
        }

        if (saveRuntimeToInventorySO)
        {
            saveRuntimeToInventorySO = false;
            SaveRuntimeToInventorySO();
        }
    }

    public void LoadInventoryFromSO()
    {
        currentInventory = new InventoryRuntime(inventorySO);
    }

    public void SaveRuntimeToInventorySO()
    {
        if (inventorySO == null || currentInventory == null)
        {
            Debug.LogWarning("Cannot save inventory. Missing InventorySO or InventoryRuntime.");
            return;
        }

        inventorySO.SaveFromRuntime(currentInventory);
        MarkInventorySOAsDirty();

        Debug.Log("Runtime inventory saved to InventorySO.");
    }

    private void MarkInventorySOAsDirty()
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(inventorySO);
        AssetDatabase.SaveAssets();
#endif
    }

    public void CheckpointSave()
    {
        SaveRuntimeToInventorySO();
    }

    public void AddItem(ItemSO itemData)
    {
        if (currentInventory == null)
            return;

        currentInventory.AddItem(itemData);
        RefreshUI();
    }

    public void ClearInventory()
    {
        if (currentInventory == null)
            return;

        currentInventory.ClearInventory();
        RefreshUI();
    }

    public void AssignItemToQuickAccess(int inventoryIndex, int quickAccessIndex)
    {
        if (currentInventory == null)
            return;

        currentInventory.AssignItemToQuickAccess(inventoryIndex, quickAccessIndex);
        RefreshUI();
    }

    public void RemoveItemFromQuickAccess(int quickAccessIndex)
    {
        if (currentInventory == null)
            return;

        currentInventory.RemoveItemFromQuickAccess(quickAccessIndex);
        RefreshUI();
    }

    public bool UseItemAt(int inventoryIndex)
    {
        if (currentInventory == null)
            return false;

        bool used = currentInventory.UseItemAt(inventoryIndex, itemUser);

        if (used)
        {
            RefreshUI();
        }

        return used;
    }

    public void UseQuickAccessItem(int quickAccessIndex)
    {
        if (currentInventory == null)
            return;

        IReadOnlyList<QuickAccessSlot> quickSlots = currentInventory.QuickAccessSlots;

        if (quickAccessIndex < 0 || quickAccessIndex >= quickSlots.Count)
            return;

        QuickAccessSlot quickSlot = quickSlots[quickAccessIndex];

        if (quickSlot == null || quickSlot.IsEmpty)
            return;

        UseItemAt(quickSlot.InventoryIndex);
    }

    public bool RemoveItem(ItemSO itemData, int amount = 1)
    {
        if (currentInventory == null)
            return false;

        bool removed = currentInventory.RemoveItem(itemData, amount);

        if (removed)
        {
            RefreshUI();
        }

        return removed;
    }

    public bool RemoveItemAt(int inventoryIndex)
    {
        if (currentInventory == null)
            return false;

        bool removed = currentInventory.RemoveItemAt(inventoryIndex);

        if (removed)
        {
            RefreshUI();
        }

        return removed;
    }

    public InventoryRuntime GetInventory()
    {
        return currentInventory;
    }

    public void RefreshUI()
    {
        if (inventoryUI != null && currentInventory != null)
        {
            inventoryUI.RefreshInventoryUI(currentInventory);
        }
    }
}