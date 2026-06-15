using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] InventorySO currentInventory;
    public static InventoryManager Instance;
    public bool clearInventory = false;
    private void Awake()
    {
        if (Instance == null) 
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (clearInventory)
        {
            clearInventory = false;
            ClearInventory();
        }
    }

    public void AddItem(ItemSO itemData) { 
        currentInventory.AddItem(itemData);
    
    }

    public void ClearInventory()
    {
        currentInventory.ClearInventory();
    }


}
