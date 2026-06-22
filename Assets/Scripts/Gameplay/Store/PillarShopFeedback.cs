using UnityEngine;

public class PillarShopFeedback : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] AudioClip _buy;
    [SerializeField] AudioClip _buyError;

    [Header("Volume")]
    [SerializeField] float _buyVolume;
    [SerializeField] float _buyVolumeError;

    PillarShop shop;
    private void Awake()
    {
        shop = GetComponent<PillarShop>();
    }

    private void OnEnable()
    {
        shop.OnBuy += OnBuy;
        shop.OnBuyError += OnBuyError;

    }

    private void OnBuy()
    {
        if (_buy == null) return;
        PlaySound(_buy, _buyVolume);
    }

    private void OnBuyError()
    {
        if (_buyError == null) return;
        PlaySound(_buyError, _buyVolumeError);
    }

    private void PlaySound(AudioClip clip, float vol)
    {
        AudioInfo audioInfo = new AudioInfo(clip, vol);
        ComponentLocatorService.Components.SfxManager.PlaySound(audioInfo, transform.position);
    }

    private void OnDisable()
    {
        shop.OnBuy += OnBuy;
        shop.OnBuyError += OnBuyError;
    }
}
