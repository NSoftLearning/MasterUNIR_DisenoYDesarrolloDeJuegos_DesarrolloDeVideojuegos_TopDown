using UnityEngine;

public class ComponentsLibrary : MonoBehaviour
{
    public InventorySO InventorySO => _inventorySlot;

    [SerializeField] InventorySO _inventorySlot;

    private void Awake()
    {
        ComponentLocatorService.BuildComponentsLibrary(this);
    }
}
