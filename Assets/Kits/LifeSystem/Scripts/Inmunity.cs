using System.Collections;
using UnityEngine;

public class Inmunity : MonoBehaviour
{
    [SerializeField] private float _inmunityTimer = 1f;
    [SerializeField] private float _blinkTime = 0.1f;
    [SerializeField] private SpriteRenderer _blinkSprite;

    private Life life;
    private bool initialized = false;

    private Coroutine postDamageInmunityCoroutine;
    private Coroutine blinkCoroutine;

    private bool blink = false;

    private void Awake()
    {
        life = GetComponent<Life>();

        if (_blinkSprite == null)
        {
            _blinkSprite = GetComponentInChildren<SpriteRenderer>();
        }
    }

    private void OnEnable()
    {
        if (life != null)
        {
            life.LifeChanged += OnLifeChanged;
        }
    }

    private void OnDisable()
    {
        if (life != null)
        {
            life.LifeChanged -= OnLifeChanged;
        }
    }

    private void OnLifeChanged(LifeChangedDTO lifeChangedDTO)
    {
        if (!initialized)
        {
            initialized = true;
            return;
        }

        if (_inmunityTimer <= 0f)
            return;

        if (lifeChangedDTO.currentValue <= 0)
            return;

        if (lifeChangedDTO.deltaValue >= 0)
            return;

        StartPostDamageInmunity();
    }

    private void StartPostDamageInmunity()
    {
        if (postDamageInmunityCoroutine != null)
        {
            StopCoroutine(postDamageInmunityCoroutine);

            if (life != null)
            {
                life.SetInmunity(false);
            }
        }

        postDamageInmunityCoroutine = StartCoroutine(PostDamageInmunityRoutine());
    }

    private IEnumerator PostDamageInmunityRoutine()
    {
        if (life != null)
        {
            life.SetInmunity(true);
        }

        PlayBlink(_inmunityTimer);

        yield return new WaitForSeconds(_inmunityTimer);

        if (life != null)
        {
            life.SetInmunity(false);
        }

        postDamageInmunityCoroutine = null;
    }

    public void PlayBlink(float duration)
    {
        if (duration <= 0f)
            return;

        if (_blinkSprite == null)
            return;

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }

        blinkCoroutine = StartCoroutine(BlinkRoutine(duration));
    }

    private IEnumerator BlinkRoutine(float duration)
    {
        blink = true;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += _blinkTime;

            _blinkSprite.enabled = !_blinkSprite.enabled;

            yield return new WaitForSeconds(_blinkTime);
        }

        blink = false;

        if (_blinkSprite != null)
        {
            _blinkSprite.enabled = true;
        }

        blinkCoroutine = null;
    }
}