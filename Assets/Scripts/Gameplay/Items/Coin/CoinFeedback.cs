using UnityEngine;

public class CoinFeedback : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] AudioClip _coin;

    [Header("Volume")]
    [SerializeField] float _coinVolume;

    Coin coin;
    private void Awake()
    {
        coin = GetComponent<Coin>();
    }

    private void OnEnable()
    {
        coin.OnCatch += OnCatchCoin;
    }

    private void OnCatchCoin()
    {
        if (_coin == null) return;
        PlaySound(_coin, _coinVolume);
    }

    private void PlaySound(AudioClip clip, float vol)
    {
        AudioInfo audioInfo = new AudioInfo(clip, vol);
        ComponentLocatorService.Components.SfxManager.PlaySound(audioInfo);
    }

    private void OnDisable()
    {
        coin.OnCatch -= OnCatchCoin;
    }
}
