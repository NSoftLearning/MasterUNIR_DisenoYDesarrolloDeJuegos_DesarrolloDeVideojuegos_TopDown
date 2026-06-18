using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private ItemSO itemData;

    private bool isCollected;

    private void Awake()
    {
        isCollected = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            Collect();
        }
    }

    public void Collect()
    {
        ComponentLocatorService.Components.InventoryManager.AddItem(itemData);
        Destroy(gameObject);
    }
}