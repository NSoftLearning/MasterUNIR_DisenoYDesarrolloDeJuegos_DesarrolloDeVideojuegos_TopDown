using TMPro;
using UnityEngine;

public class PillarShop : MonoBehaviour, IInteractables
{
    [SerializeField] ItemSO itemData;
    //[SerializeField] InventorySO inventoryData;
    [SerializeField] Animator anim;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] bool isSelected;

    CoinManager coinManager;
    
    void Awake()
    {
        coinManager = FindAnyObjectByType<CoinManager>();
        anim = GetComponent<Animator>();

        if (priceText == null) { priceText = GameObject.Find("PriceText").GetComponent<TextMeshProUGUI>(); }

        priceText.text = ("$" + itemData.price.ToString());

        
    }
   

    public void StartInteraction()
    {
        if (coinManager.currentCoins < itemData.price)
        {
            Debug.Log("Saldo insuficiente");
            anim.SetTrigger("Missing");
            return;
        }

        coinManager.currentCoins -= itemData.price;
        anim.SetTrigger("Buy It");

       InventoryManager.Instance.AddItem(itemData);
       // Agregar función de agregado del item al inventario

        Debug.Log("Compraste: " + itemData.itemName);

    }

    public void Select()
    {
        anim.SetBool("ShowText", true);

        Debug.Log("Pilar seleccionado");
    }

    public void Unselect()
    {
        anim.SetBool("ShowText", false);

        Debug.Log("Pilar deseleccionado");
    }
}
