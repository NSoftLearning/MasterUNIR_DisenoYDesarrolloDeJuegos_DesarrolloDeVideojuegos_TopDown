using UnityEngine;

public class ChestFeedback : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    [SerializeField] float volume;

    Chest chest;
    private void Awake()
    {
        chest = GetComponent<Chest>();
    }

    private void OnEnable()
    {
        chest.eventoEfecto += OnOpen;
    }

    private void OnOpen()
    {
        AudioInfo audioInfo = new AudioInfo();
        ComponentLocatorService.Components.SfxManager.PlaySound(audioInfo, transform.position);
    }

    private void OnDisable()
    {
        chest.eventoEfecto -= OnOpen;
    }
}
