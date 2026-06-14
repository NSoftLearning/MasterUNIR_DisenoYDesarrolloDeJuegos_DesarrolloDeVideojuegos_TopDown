using System.Xml.Serialization;
using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    [SerializeField] CoinManager coinManager;
    [SerializeField] bool isCollected = false;
    [SerializeField] Canvas canvas;
    [SerializeField] RectTransform coinPointTransfer;
    [SerializeField] float flySpeed = 8f;
    private Collider2D coinCollider;

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

        coinCollider = GetComponent<CapsuleCollider2D>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            coinCollider.enabled = false; //Collider físico, para que no choque con el jugador.

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
        Destroy(gameObject);
    }
}
