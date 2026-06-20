using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "Content/Inventory/InventoryInstance")]
public class InventorySO : ScriptableObject
{
    [Header("Saved Inventory")]
    [SerializeField] private List<InventorySlot> savedSlots = new List<InventorySlot>();

    [Header("Saved Quick Access")]
    [SerializeField] private List<int> savedQuickAccessIndexes = new List<int>();

    [Header("Saved Weapons")]
    [SerializeField] private List<WeaponData> savedWeapons = new List<WeaponData>();
    [SerializeField] private WeaponData savedEquippedWeapon;

    [Header("Inventory Settings")]
    [SerializeField] private bool allowStacking = true;

    [Header("Quick Access Settings")]
    [SerializeField] private int quickAccessSlotCount = 2;

    public IReadOnlyList<InventorySlot> SavedSlots => savedSlots;
    public IReadOnlyList<int> SavedQuickAccessIndexes => savedQuickAccessIndexes;
    public IReadOnlyList<WeaponData> SavedWeapons => savedWeapons;
    public WeaponData SavedEquippedWeapon => savedEquippedWeapon;

    public bool AllowStacking => allowStacking;
    public int QuickAccessSlotCount => quickAccessSlotCount;

    public void SaveFromRuntime(InventoryRuntime runtime)
    {
        if (runtime == null)
            return;

        SaveInventorySlots(runtime);
        SaveQuickAccessSlots(runtime);
        SaveWeapons(runtime);
    }

    private void SaveInventorySlots(InventoryRuntime runtime)
    {
        savedSlots.Clear();

        IReadOnlyList<InventorySlot> runtimeSlots = runtime.InventorySlots;

        for (int i = 0; i < runtimeSlots.Count; i++)
        {
            InventorySlot runtimeSlot = runtimeSlots[i];

            if (runtimeSlot == null || runtimeSlot.IsEmpty)
                continue;

            InventorySlot copy = new InventorySlot();
            copy.InitializeSlot(runtimeSlot.Item, runtimeSlot.ItemsInSlot);

            savedSlots.Add(copy);
        }
    }

    private void SaveQuickAccessSlots(InventoryRuntime runtime)
    {
        savedQuickAccessIndexes.Clear();

        IReadOnlyList<QuickAccessSlot> runtimeQuickSlots = runtime.QuickAccessSlots;

        for (int i = 0; i < runtimeQuickSlots.Count; i++)
        {
            QuickAccessSlot quickSlot = runtimeQuickSlots[i];

            if (quickSlot == null || quickSlot.IsEmpty)
            {
                savedQuickAccessIndexes.Add(-1);
            }
            else
            {
                savedQuickAccessIndexes.Add(quickSlot.InventoryIndex);
            }
        }
    }

    private void SaveWeapons(InventoryRuntime runtime)
    {
        savedWeapons.Clear();

        IReadOnlyList<WeaponData> runtimeWeapons = runtime.Weapons;

        for (int i = 0; i < runtimeWeapons.Count; i++)
        {
            WeaponData weapon = runtimeWeapons[i];

            if (weapon == null)
                continue;

            if (savedWeapons.Contains(weapon))
                continue;

            savedWeapons.Add(weapon);
        }

        savedEquippedWeapon = runtime.EquippedWeapon;
    }
}