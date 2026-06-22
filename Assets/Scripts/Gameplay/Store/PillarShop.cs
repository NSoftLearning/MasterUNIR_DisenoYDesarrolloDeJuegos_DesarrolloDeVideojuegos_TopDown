using TMPro;
using UnityEngine;
using System.Collections;
using System;

public class PillarShop : MonoBehaviour, IInteractables
{
    [Header("Item Sale")]
    [SerializeField] private ItemSO itemData;

    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private TextMeshProUGUI priceText;

    [Header("Feedback Key")]
    [SerializeField] private SpriteRenderer spriteKey;
    [SerializeField] private float fadeShowKey = 0.2f;

    private InventoryManager inventoryManager;
    private Coroutine fadeRoutine;

    public event Action OnBuy;
    public event Action OnBuyError;

    private void Awake()
    {
        ResolveInventoryManager();

        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }

        if (priceText == null)
        {
            GameObject priceTextObject = GameObject.Find("PriceText");

            if (priceTextObject != null)
            {
                priceText = priceTextObject.GetComponent<TextMeshProUGUI>();
            }
        }

        RefreshPriceText();

        if (spriteKey == null)
        {
            Debug.LogWarning("No se ha asignado un SpriteRenderer del Keyboard para el feedback visual del pilar de tienda.");
        }
        else
        {
            spriteKey.color = new Color(spriteKey.color.r, spriteKey.color.g, spriteKey.color.b, 0f);
        }
    }

    private void ResolveInventoryManager()
    {
        if (inventoryManager != null)
            return;

        if (ComponentLocatorService.Components != null &&
            ComponentLocatorService.Components.InventoryManager != null)
        {
            inventoryManager = ComponentLocatorService.Components.InventoryManager;
            return;
        }

        inventoryManager = ComponentLocatorService.Components.InventoryManager;
    }

    private void RefreshPriceText()
    {
        if (priceText == null)
            return;

        if (itemData == null)
        {
            priceText.text = "$0";
            return;
        }

        priceText.text = "$" + itemData.price.ToString();
    }

    public void StartInteraction()
    {
        if (itemData == null)
        {
            Debug.LogWarning("No se ha asignado ningún item al PillarShop.");
            OnBuyError?.Invoke();
            return;
        }

        if (inventoryManager == null)
        {
            ResolveInventoryManager();
        }

        if (inventoryManager == null)
        {
            Debug.LogWarning("PillarShop no ha podido encontrar InventoryManager.");
            OnBuyError?.Invoke();
            return;
        }

        bool paidSuccessfully = inventoryManager.SpendCoins(itemData.price);

        if (!paidSuccessfully)
        {
            Debug.Log("Saldo insuficiente");

            if (anim != null)
            {
                anim.SetTrigger("Missing");
            }

            OnBuyError?.Invoke();
            return;
        }

        inventoryManager.AddItem(itemData);

        if (anim != null)
        {
            anim.SetTrigger("Buy It");
        }

        OnBuy?.Invoke();

        Debug.Log("Compraste: " + itemData.itemName);
    }

    public void Select()
    {
        

        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = StartCoroutine(FadeInVisual());

        Debug.Log("Pilar seleccionado");
    }

    public void Unselect()
    {
        

        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = StartCoroutine(FadeOutVisual());

        Debug.Log("Pilar deseleccionado");
    }

    private IEnumerator FadeInVisual()
    {
        if (anim != null)
        {
            anim.SetBool("ShowText", true);
        }

        if (spriteKey == null)
            yield break;

        float alpha = spriteKey.color.a;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime / fadeShowKey;
            alpha = Mathf.Clamp01(alpha);

            spriteKey.color = new Color(
                spriteKey.color.r,
                spriteKey.color.g,
                spriteKey.color.b,
                alpha
            );

            yield return null;
        }

        fadeRoutine = null;
    }

    private IEnumerator FadeOutVisual()
    {
        if (anim != null)
        {
            anim.SetBool("ShowText", false);
        }

        if (spriteKey == null)
            yield break;

        float alpha = spriteKey.color.a;

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime / fadeShowKey;
            alpha = Mathf.Clamp01(alpha);

            spriteKey.color = new Color(
                spriteKey.color.r,
                spriteKey.color.g,
                spriteKey.color.b,
                alpha
            );

            yield return null;
        }

        fadeRoutine = null;
    }
}