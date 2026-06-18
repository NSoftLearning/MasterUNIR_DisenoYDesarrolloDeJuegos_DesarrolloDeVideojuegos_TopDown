using UnityEngine;

public class TrapFeedback : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] AudioClip _trapSound;
    [SerializeField] AudioClip _trapDeactivateSound;

    [Header("Volume")]
    [SerializeField] float _trapVolume;
    [SerializeField] float _trapDeactivateSoundVolume;

    ITrap trap;
    private void Awake()
    {
        trap = GetComponent<ITrap>();
    }

    private void OnEnable()
    {
        trap.OnActivate += OnActivate;
        trap.OnDeactivate += OnDeactivate;
    }

    private void OnActivate()
    {
        if (_trapSound == null) return;

        AudioInfo trap = new AudioInfo(_trapSound, _trapVolume);
        ComponentLocatorService.Components.SfxManager.PlaySound(trap);
    }

    private void OnDeactivate()
    {
        if (_trapDeactivateSound == null) return;

        AudioInfo trap = new AudioInfo(_trapDeactivateSound, _trapDeactivateSoundVolume);
        ComponentLocatorService.Components.SfxManager.PlaySound(trap);
    }

    private void OnDisable()
    {
        trap.OnActivate -= OnActivate;
        trap.OnDeactivate -= OnDeactivate;
    }
}
