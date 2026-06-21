using UnityEngine;
using System.Collections;
using System;

public class Coin : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private bool isCollected = false;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform coinPointTransfer;
    [SerializeField] private float flySpeed = 8f;
    [SerializeField] private float delayBeforeCollect = 1f;
    [SerializeField] private Collider2D coinCollider;

    [Header("Coin Value")]
    [SerializeField] private int coinValue = 1;

    public event Action OnCatch;

    private bool canTake = false;

    private void Start()
    {
        ResolveInventoryManager();

        if (coinPointTransfer == null)
        {
            GameObject coinTargetObject = GameObject.Find("CoinTarget");

            if (coinTargetObject != null)
            {
                coinPointTransfer = coinTargetObject.GetComponent<RectTransform>();
            }
        }

        if (coinCollider == null)
        {
            coinCollider = GetComponentInChildren<Collider2D>();
        }

        if (coinPointTransfer != null)
        {
            canvas = coinPointTransfer.GetComponentInParent<Canvas>();
        }

        canTake = false;
        StartCoroutine(DelayBeforeCollect());
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

        inventoryManager = InventoryManager.Instance;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCollected && canTake)
        {
            isCollected = true;

            if (coinCollider != null)
            {
                coinCollider.enabled = false;
            }

            StartCoroutine(MoveToTarget());
        }
    }

    private IEnumerator MoveToTarget()
    {
        if (coinPointTransfer == null || canvas == null)
        {
            CollectCoin();
            yield break;
        }

        Vector3 worldTarget;

        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            coinPointTransfer,
            RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, coinPointTransfer.position),
            canvas.worldCamera,
            out worldTarget
        );

        while (Vector3.Distance(transform.position, worldTarget) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                worldTarget,
                flySpeed * Time.deltaTime
            );

            yield return null;
        }

        CollectCoin();
    }

    private void CollectCoin()
    {
        if (inventoryManager != null)
        {
            inventoryManager.AddCoins(coinValue);
        }
        else
        {
            Debug.LogWarning("Coin could not find InventoryManager. Coin was not added.");
        }

        OnCatch?.Invoke();
        Destroy(gameObject);
    }

    private IEnumerator DelayBeforeCollect()
    {
        yield return new WaitForSeconds(delayBeforeCollect);
        canTake = true;
    }
}