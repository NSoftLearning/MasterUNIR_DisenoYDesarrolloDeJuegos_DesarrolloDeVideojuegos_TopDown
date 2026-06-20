using UnityEngine;

public class LeverFeedback : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] AudioClip _lever;

    [Header("Volume")]
    [SerializeField] float _leverVolume;

    Lever lever;
    private void Awake()
    {
        lever = GetComponent<Lever>();
    }

    private void OnEnable()
    {
        lever.OnPress += OnPressLever;
    }

    private void OnPressLever()
    {
        PlaySound(_lever, _leverVolume);
    }

    private void PlaySound(AudioClip clip, float vol)
    {
        AudioInfo audioInfo = new AudioInfo(clip, vol);
        ComponentLocatorService.Components.SfxManager.PlaySound(audioInfo);
    }

    private void OnDisable()
    {
        lever.OnPress -= OnPressLever;
    }
}
