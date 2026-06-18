using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chest : MonoBehaviour, IInteractables
{
    [Header("Chest Settings")]
    [SerializeField] Animator anim;
    [SerializeField] bool isOpen = false;
    [SerializeField] SpriteRenderer spriteKey;
    [SerializeField] float fadeShowKey = 0.2f;

    [Header("Chest Event")]
    public UnityEvent eventoEfecto;

    void Awake()
    {
        anim = GetComponent<Animator>();
        isOpen = false;

        if (spriteKey == null)
        {
            spriteKey = GetComponentInChildren<SpriteRenderer>();
        }

        spriteKey.color = new Color(spriteKey.color.r, spriteKey.color.g, spriteKey.color.b, 0f);
    }

    public void Select()
    {
        if (!isOpen)
        {
            StopAllCoroutines();
            StartCoroutine(FadeInKey());
            Debug.Log("Cofre seleccionado");
        }
            
    }

    public void Unselect()
    {
        if (!isOpen)
        {
            StopAllCoroutines();
            StartCoroutine(FadeOutKey());
            Debug.Log("Cofre deseleccionado");
        }
    }

    public void StartInteraction()
    {
        if (!isOpen)
        {
            isOpen = true;
            anim.SetTrigger("Action");
            Debug.Log("Cofre abierto");

            eventoEfecto?.Invoke();

            StopAllCoroutines();
            StartCoroutine(FadeOutKey());
        }
        else
        {
            Debug.Log("El cofre ya está abierto");
        }
    }

    IEnumerator FadeInKey()
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime / fadeShowKey;
            spriteKey.color = new Color(spriteKey.color.r, spriteKey.color.g, spriteKey.color.b, alpha);
            yield return null;
        }
        alpha = 1f;
    }

    IEnumerator FadeOutKey()
    {
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime / fadeShowKey;
            spriteKey.color = new Color(spriteKey.color.r, spriteKey.color.g, spriteKey.color.b, alpha);
            yield return null;
        }
        alpha = 0f;
    }
}

