using System.Collections;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [Header("Destruction Settings")]
    [SerializeField] float destroyDelay = 0.5f;

    [Header("Object References")]
    [SerializeField] SpriteRenderer spriteRender;
    [SerializeField] SplashItem splashItem;
    [SerializeField] Collider2D destructibleCollider;

    private bool isDestroyed = false;

    private void Awake()
    {
        isDestroyed = false;
        destructibleCollider = GetComponent<Collider2D>();
        destructibleCollider.enabled = true;
        spriteRender = GetComponent<SpriteRenderer>();
        splashItem = GetComponentInChildren<SplashItem>();
    }

    public void DestroyObject()
    {
       if (!isDestroyed) 
       {
            isDestroyed = true;
            spriteRender.enabled = false;
            destructibleCollider.enabled = false;

            splashItem.SpawnSplash();
            Destroy(gameObject);
       }     
    }
}
