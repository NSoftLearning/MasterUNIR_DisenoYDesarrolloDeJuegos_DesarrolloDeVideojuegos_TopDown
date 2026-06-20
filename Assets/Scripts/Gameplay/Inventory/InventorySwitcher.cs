using UnityEngine;

public class InventorySwitcher : MonoBehaviour
{
    [SerializeField] private InventorySO InventorySO;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InventoryManager manager = ComponentLocatorService.Components.InventoryManager;
            if (manager.GetInventory().Equals(InventorySO))
                return;
            manager.SaveRuntimeToInventorySO();
            //manager.SwitchInventorySO(InventorySO);
        }
    }
}
