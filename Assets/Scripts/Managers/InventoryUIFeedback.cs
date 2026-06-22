using UnityEngine;

public class InventoryUIFeedback : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] AudioClip _openBag;
    [SerializeField] AudioClip _closeBag;

    [Header("Volume")]
    [SerializeField] float _openBagVolume;
    [SerializeField] float _closeBagVolume;

    InventoryUIManager inv;
    private void Awake()
    {
        inv = GetComponent<InventoryUIManager>();
    }

    private void OnEnable()
    {
        inv.OnOpenBag += OnOpenBag;
        inv.OnCloseBag += OnCloseBag;
    }

    private void OnOpenBag()
    {
        PlaySound(_openBag, _openBagVolume);
    }

    private void OnCloseBag()
    {
        PlaySound(_openBag, _closeBagVolume);
    }

    private void PlaySound(AudioClip clip, float vol)
    {
        if (clip == null) return;

        AudioInfo audioInfo = new AudioInfo(clip, vol);
        ComponentLocatorService.Components.SfxManager.PlaySound(audioInfo, transform.position);
    }

    private void OnDisable()
    {
        inv.OnOpenBag -= OnOpenBag;
        inv.OnCloseBag -= OnCloseBag;
    }
}
