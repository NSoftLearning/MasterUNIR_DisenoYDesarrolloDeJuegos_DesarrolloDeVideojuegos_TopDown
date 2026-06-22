using UnityEngine;

public class InventoryReseter : MonoBehaviour
{

    [SerializeField] bool resetOnStart = true;

    private void Start()
    {
        if (resetOnStart)
        {
            ResetInventory();
        }
    }
    public void ResetInventory()
    {
        ComponentLocatorService.Components.InventoryManager.ClearInventory();
        ComponentLocatorService.Components.InventoryManager.SaveRuntimeToInventorySO();
    }
}
