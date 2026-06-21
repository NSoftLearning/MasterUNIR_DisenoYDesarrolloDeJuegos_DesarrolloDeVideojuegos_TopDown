using UnityEngine;

public class CollectibleWeapon : MonoBehaviour
{
    [SerializeField] private WeaponData _weaponData;

    private bool isCollected = false;

    public WeaponData GetWeapon()
    {
        return _weaponData;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected)
            return;

        if (!collision.CompareTag("Player"))
            return;

        if (_weaponData == null)
        {
            Debug.LogWarning("CollectibleWeapon has no WeaponData assigned.");
            return;
        }

        isCollected = true;

        Collect(collision.gameObject);
    }

    private void Collect(GameObject player)
    {
        PlayerWeaponController weaponController = player.GetComponent<PlayerWeaponController>();

        if (weaponController != null)
        {
            weaponController.NewWeapon(gameObject);
        }
        else if (ComponentLocatorService.Components.InventoryManager != null)
        {
            ComponentLocatorService.Components.InventoryManager.AddWeapon(_weaponData);
            ComponentLocatorService.Components.InventoryManager.EquipWeapon(_weaponData);
        }
        else
        {
            Debug.LogWarning("No PlayerWeaponController or InventoryManager found to collect weapon.");
            isCollected = false;
            return;
        }

        Destroy(gameObject);
    }
}
