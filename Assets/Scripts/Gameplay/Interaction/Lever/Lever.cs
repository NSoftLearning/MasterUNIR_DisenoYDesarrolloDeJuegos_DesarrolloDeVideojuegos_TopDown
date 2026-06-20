using System.Collections;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractables
{
    [Header("Lever Settings")]
    [SerializeField] Animator anim;
    [SerializeField] bool blocked = false;
    [SerializeField] public bool activated = false;
    [SerializeField] SpriteRenderer spriteKey;
    [SerializeField] float fadeShowKey = 0.2f;

    Coroutine fadeRoutine;

    void Awake()
    {
        anim = GetComponent<Animator>();
        activated = false;

        if (spriteKey == null)
        {
            spriteKey = GetComponentInChildren<SpriteRenderer>();
        }

        spriteKey.color = new Color(spriteKey.color.r, spriteKey.color.g, spriteKey.color.b, 0f);
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


        Debug.Log("Palanca seleccionada");
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
        Debug.Log("Palanca deseleccionada");
    }

    public void StartInteraction()
    {

        if (blocked)
        {
            anim.SetTrigger("Blocked");
            Debug.Log("La palanca está bloqueada");
            return;
        }
        else
        {

            if (activated)
            {
                activated = false;
                anim.SetBool("Activate", false);
                Debug.Log("La palanca ya está activada");
                return;
            }
            else
            {
                activated = true;
                anim.SetBool("Activate", true);
            }
                

        }

    }

    IEnumerator FadeInVisual()
    {
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

