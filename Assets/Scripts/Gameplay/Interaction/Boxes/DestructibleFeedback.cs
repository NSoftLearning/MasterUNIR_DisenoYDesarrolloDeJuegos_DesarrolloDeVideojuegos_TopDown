using UnityEngine;

public class DestructibleFeedback : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] AudioClip _destructible;

    [Header("Volume")]
    [SerializeField] float _destructibleVolume;

    Destructible destructible;
    private void Awake()
    {
        destructible = GetComponent<Destructible>();
    }

    private void OnEnable()
    {
        destructible.BlockerStatusChanged += OnPressLever;
    }

    private void OnPressLever()
    {
        if (_destructible == null) return;
        PlaySound(_destructible, _destructibleVolume);
    }

    private void PlaySound(AudioClip clip, float vol)
    {
        AudioInfo audioInfo = new AudioInfo(clip, vol);
        ComponentLocatorService.Components.SfxManager.PlaySound(audioInfo, transform.position);
    }

    private void OnDisable()
    {
        destructible.BlockerStatusChanged -= OnPressLever;
    }
}
