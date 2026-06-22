using System;
using System.Collections;
using UnityEngine;

public class Destructible : MonoBehaviour, IPathFindingBlocker
{
    [Header("Object References")]
    [SerializeField] SpriteRenderer spriteRender;
   // [SerializeField] SplashItem splashItem;
    [SerializeField] Collider2D destructibleCollider;

    private bool isDestroyed = false;

    public event Action BlockerStatusChanged;

    private void Awake()
    {
        isDestroyed = false;
        destructibleCollider = GetComponent<Collider2D>();
        destructibleCollider.enabled = true;
        spriteRender = GetComponent<SpriteRenderer>();
       // splashItem = GetComponentInChildren<SplashItem>();
    }

    [ContextMenu(nameof(DestroyObject))]
    public void DestroyObject()
    {
       if (!isDestroyed) 
       {
            isDestroyed = true;
            spriteRender.enabled = false;
            destructibleCollider.enabled = false;

            BlockerStatusChanged?.Invoke();

           // splashItem.SpawnSplash();
            Destroy(gameObject);

       }     
    }
}
