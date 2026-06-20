using System.Xml.Serialization;
using UnityEngine;
using System.Collections;
using System;

public class Coin : MonoBehaviour
{
    [SerializeField] CoinManager coinManager;
    [SerializeField] bool isCollected = false;
    [SerializeField] Canvas canvas;
    [SerializeField] RectTransform coinPointTransfer;
    [SerializeField] float flySpeed = 8f;
    [SerializeField] float delayBeforeCollect = 1f;
    [SerializeField] Collider2D coinCollider;

    public event Action OnCatch;

    private bool canTake = false;

    private void Start()
    {
        if (coinManager == null)
        {
            coinManager = FindAnyObjectByType<CoinManager>();
        }

        if (coinPointTransfer == null)
        {
            coinPointTransfer = GameObject.Find("CoinTarget").GetComponent<RectTransform>();
        }

        coinCollider = GetComponentInChildren<CircleCollider2D>();

        canvas = coinPointTransfer.GetComponentInParent<Canvas>();

        canTake = false;

        StartCoroutine(DelayBeforeCollect());

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCollected && canTake)
        {
            isCollected = true;
            coinCollider.enabled = false; // Evita que se pegue en los muros

            StartCoroutine(MoveToTarget());
        }
    }

    IEnumerator MoveToTarget()
    {
        Vector3 worldTarget;

        RectTransformUtility.ScreenPointToWorldPointInRectangle(coinPointTransfer, RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, coinPointTransfer.position), canvas.worldCamera, out worldTarget);       
        
        while (Vector3.Distance(transform.position, worldTarget) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, worldTarget, flySpeed * Time.deltaTime);

            yield return null;
        }

        coinManager.AddCoin();
        OnCatch?.Invoke();
        Destroy(gameObject);
    }

    IEnumerator DelayBeforeCollect()
    {
        yield return new WaitForSeconds(delayBeforeCollect);

        canTake = true;

    }
}
