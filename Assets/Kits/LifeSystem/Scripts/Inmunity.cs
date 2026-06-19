using System.Collections;
using UnityEngine;

public class Inmunity : MonoBehaviour
{
    [SerializeField] float _inmunityTimer = 1f;
    [SerializeField] float _blinkTime = 0.1f;
    [SerializeField] SpriteRenderer _blinkSprite;

    Life life;
    private void Awake()
    {
        life = GetComponent<Life>();
    }

    private void OnEnable()
    {
        life.LifeChanged += OnLifeChanged;
    }

    private void OnLifeChanged(LifeChangedDTO lifeChangedDTO)
    {
        if (_inmunityTimer == 0 || lifeChangedDTO.currentValue <= 0) return;

        StopAllCoroutines();
        StartCoroutine(Inmune());
    }

    bool blink = false;
    IEnumerator Inmune()
    {
        life.SetInmunity(true);
        blink = true;
        StartCoroutine(Blink());

        yield return new WaitForSeconds(_inmunityTimer);

        blink = false;
        life.SetInmunity(false);
    }

    IEnumerator Blink()
    {
        while (blink)
        {
            yield return new WaitForSeconds(_blinkTime);
            _blinkSprite.enabled = !_blinkSprite.enabled;
        }
    }

    private void OnDisable()
    {
        life.LifeChanged -= OnLifeChanged;
    }
}
