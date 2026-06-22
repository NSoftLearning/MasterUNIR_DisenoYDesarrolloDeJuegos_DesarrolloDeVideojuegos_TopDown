using System;
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
    public Action eventoEfecto;

    Coroutine fadeRoutine;
    SplashItem splash;


    void Awake()
    {
        anim = GetComponent<Animator>();
        splash = GetComponentInChildren<SplashItem>();

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
            if (fadeRoutine != null)
            {
                StopCoroutine(fadeRoutine);
                fadeRoutine = StartCoroutine(FadeInVisual());
            }
            else
            {
                fadeRoutine = StartCoroutine(FadeInVisual());
            }
        }
        
    }

    public void Unselect()
    {
        if (!isOpen)
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

            splash.SpawnSplash();

            if (fadeRoutine != null)
            {
                StopCoroutine(fadeRoutine);
                fadeRoutine = StartCoroutine(FadeOutVisual());
            }
            else
            {
                fadeRoutine = StartCoroutine(FadeOutVisual());
            }
        }
        else
        {
            Debug.Log("El cofre ya está abierto");
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

