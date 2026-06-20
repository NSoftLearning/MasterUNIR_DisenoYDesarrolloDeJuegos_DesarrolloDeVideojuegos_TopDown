using TMPro;
using UnityEngine;
using System.Collections;
using System;

public class PillarShop : MonoBehaviour, IInteractables
{
    [Header("Item Sale")]
    [SerializeField] ItemSO itemData;

    [Header("References")]
    [SerializeField] Animator anim;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] bool isSelected;

    [Header("Feddback Key")]
    [SerializeField] SpriteRenderer spriteKey;
    [SerializeField] float fadeShowKey = 0.2f;

    CoinManager coinManager;
    Coroutine fadeRoutine;

    public event Action OnBuy;
    public event Action OnBuyError;


    void Awake()
    {
        coinManager = FindAnyObjectByType<CoinManager>();
        anim = GetComponent<Animator>();

        if (priceText == null) { priceText = GameObject.Find("PriceText").GetComponent<TextMeshProUGUI>(); }

        priceText.text = ("$" + itemData.price.ToString());

        if (spriteKey == null)
        {
            Debug.LogWarning("No se ha asignado un SpriteRenderer del Keyboard para el feedback visual del pilar de tienda.");
        }
        else
        {
            spriteKey.color = new Color(spriteKey.color.r, spriteKey.color.g, spriteKey.color.b, 0f);
        }

    }
   

    public void StartInteraction()
    {
        if (coinManager.currentCoins < itemData.price)
        {
            Debug.Log("Saldo insuficiente");
            anim.SetTrigger("Missing");
            OnBuyError?.Invoke();
            return;
        }

        OnBuy?.Invoke();
        coinManager.currentCoins -= itemData.price;
        anim.SetTrigger("Buy It");

        ComponentLocatorService.Components.InventoryManager.AddItem(itemData);
       // Agregar función de agregado del item al inventario

        Debug.Log("Compraste: " + itemData.itemName);

    }

    public void Select()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            fadeRoutine = StartCoroutine(FadeInVisual());
        }
        else
        {
            fadeRoutine = StartCoroutine(FadeInVisual());
        }

            Debug.Log("Pilar seleccionado");
    }

    public void Unselect()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            fadeRoutine = StartCoroutine(FadeOutVisual());
        }
        else
        {
            fadeRoutine = StartCoroutine(FadeOutVisual());
        }

            Debug.Log("Pilar deseleccionado");
    }

    IEnumerator FadeInVisual()
    {
        anim.SetBool("ShowText", true);

        float alpha = spriteKey.color.a;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime / fadeShowKey;
            spriteKey.color = new Color(spriteKey.color.r, spriteKey.color.g, spriteKey.color.b, alpha);
            yield return null;
        }
        alpha = 1f;
    }

    IEnumerator FadeOutVisual()
    {
        anim.SetBool("ShowText", false);

        float alpha = spriteKey.color.a;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime / fadeShowKey;
            spriteKey.color = new Color(spriteKey.color.r, spriteKey.color.g, spriteKey.color.b, alpha);
            yield return null;
        }
        alpha = 0f;
    }
}

