#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InventoryUIManager inventoryUI;
    [SerializeField] private InventorySO inventorySO;
    [SerializeField] private GameObject itemUser;

    [Header("Weapon")]
    [SerializeField] private PlayerWeaponController playerWeaponController;

    private InventoryRuntime currentInventory;

    

    public event Action OnBasicClick;
    public event Action<ItemSO, bool> OnUse;
    public event Action<bool> OnMove;

    [Header("Debug Settings")]
    public bool clearInventory = false;
    public bool updateQuickAccess = false;
    public bool saveRuntimeToInventorySO = false;
    public bool changeNextWeapon = false;

    public int inventoryIndexToSetInFirstQuickAccess = 0;
    public int inventoryIndexToSetInSecondQuickAccess = 1;

    private void Awake()
    {
        
        LoadInventoryFromSO();
        ApplySavedWeaponToPlayer();
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

        if (changeNextWeapon)
        {
            changeNextWeapon = false;
            EquipNextWeapon();
        }
    }

    public void LoadInventoryFromSO()
    {
        currentInventory = new InventoryRuntime(inventorySO);
    }

    private void ApplySavedWeaponToPlayer()
    {
        if (playerWeaponController == null || currentInventory == null)
            return;

        if (currentInventory.EquippedWeapon == null)
            return;

        playerWeaponController.SetWeapons(currentInventory.Weapons);
        playerWeaponController.EquipWeapon(currentInventory.EquippedWeapon);
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

    public int GetCoins()
    {
        if (currentInventory == null)
            return 0;

        return currentInventory.Coins;
    }

    public void AddCoins(int amount)
    {
        if (currentInventory == null)
            return;

        currentInventory.AddCoins(amount);
        RefreshUI();
    }

    public bool SpendCoins(int amount)
    {
        if (currentInventory == null)
            return false;

        bool spent = currentInventory.SpendCoins(amount);

        if (spent)
        {
            RefreshUI();
        }

        return spent;
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

        if (playerWeaponController != null)
        {
            playerWeaponController.ClearWeapon();
        }

        RefreshUI();
    }

    public void AddWeapon(WeaponData weaponData)
    {
        if (currentInventory == null)
            return;

        bool added = currentInventory.AddWeapon(weaponData);

        if (added && currentInventory.EquippedWeapon == null)
        {
            EquipWeapon(weaponData);
            return;
        }

        RefreshUI();
    }

    public void EquipWeapon(WeaponData weaponData)
    {
        if (currentInventory == null)
            return;

        currentInventory.EquipWeapon(weaponData);

        if (playerWeaponController != null)
        {
            playerWeaponController.SetWeapons(currentInventory.Weapons);
            playerWeaponController.EquipWeapon(weaponData);
        }

        RefreshUI();
    }

    public void EquipNextWeapon()
    {
        if (currentInventory == null)
            return;

        currentInventory.EquipNextWeapon();

        if (playerWeaponController != null && currentInventory.EquippedWeapon != null)
        {
            playerWeaponController.SetWeapons(currentInventory.Weapons);
            playerWeaponController.EquipWeapon(currentInventory.EquippedWeapon);
        }

        RefreshUI();
    }

    public void AssignItemToQuickAccess(int inventoryIndex, int quickAccessIndex)
    {
        if (currentInventory == null)
        {
            NotifyMove(false);
            return;
        }

        bool moved = currentInventory.AssignItemToQuickAccess(inventoryIndex, quickAccessIndex);

        NotifyMove(moved);

        if (moved)
        {
            RefreshUI();
        }
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
        {
            NotifyUse(null, false);
            return false;
        }

        if (itemUser == null)
        {
            Debug.LogWarning("Cannot use item. ItemUser is not assigned in InventoryManager.");
            NotifyUse(null, false);
            return false;
        }

        InventorySlot slot = currentInventory.GetInventorySlot(inventoryIndex);

        if (slot == null || slot.IsEmpty)
        {
            NotifyUse(null, false);
            return false;
        }

        ItemSO itemUsed = slot.Item;

        bool used = currentInventory.UseItemAt(inventoryIndex, itemUser);

        NotifyUse(itemUsed, used);

        if (used)
        {
            RefreshUI();
        }

        return used;
    }

    public void UseQuickAccessItem(int quickAccessIndex)
    {
        if (currentInventory == null)
        {
            NotifyUse(null, false);
            return;
        }

        IReadOnlyList<QuickAccessSlot> quickSlots = currentInventory.QuickAccessSlots;

        if (quickSlots == null)
        {
            NotifyUse(null, false);
            return;
        }

        if (quickAccessIndex < 0 || quickAccessIndex >= quickSlots.Count)
        {
            NotifyUse(null, false);
            return;
        }

        QuickAccessSlot quickSlot = quickSlots[quickAccessIndex];

        if (quickSlot == null || quickSlot.IsEmpty)
        {
            NotifyUse(null, false);
            return;
        }

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

    public void NotifyBasicClick()
    {
        OnBasicClick?.Invoke();
    }

    private void NotifyUse(ItemSO item, bool isValid)
    {
        OnUse?.Invoke(item, isValid);
    }

    private void NotifyMove(bool isValid)
    {
        OnMove?.Invoke(isValid);
    }

    public void ToggleInventory()
    {
        if (inventoryUI == null)
            return;

        inventoryUI.ToggleInventory();
    }
}