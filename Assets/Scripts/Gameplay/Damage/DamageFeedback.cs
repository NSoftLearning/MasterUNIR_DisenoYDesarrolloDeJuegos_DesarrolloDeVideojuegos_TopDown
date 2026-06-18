using UnityEngine;

public class DamageFeedback : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] AudioClip _damage;
    [SerializeField] AudioClip _death;
    [SerializeField] AudioClip _heal;

    [Header("Volume")]
    [SerializeField] float _damageVolume;
    [SerializeField] float _deathVolume;
    [SerializeField] float _healVolume;

    IDamageReceiver _receiver;
    private void Awake()
    {
        _receiver = GetComponent<IDamageReceiver>();
    }

    private void OnEnable()
    {
        _receiver.LifeChanged += OnLifeChanged;
        _receiver.Died += OnDied;
    }

    private void OnLifeChanged(LifeChangedDTO lifeDTO)
    {
        float amount = lifeDTO.deltaValue;

        if (amount > 0)
        {
            AudioInfo heal= new AudioInfo(_heal, _healVolume);
            ComponentLocatorService.Components.SfxManager.PlaySound(heal);
        }
        else
        {
            AudioInfo damage = new AudioInfo(_damage, _damageVolume);
            ComponentLocatorService.Components.SfxManager.PlaySound(damage);
        }
    }

    private void OnDied()
    {
        AudioInfo death = new AudioInfo(_death, _deathVolume);
        ComponentLocatorService.Components.SfxManager.PlaySound(death);
    }

    private void OnDisable()
    {
        _receiver.LifeChanged -= OnLifeChanged;
        _receiver.Died -= OnDied;
    }
}
