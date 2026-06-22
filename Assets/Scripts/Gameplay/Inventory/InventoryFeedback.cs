using UnityEngine;

public class InventoryFeedback : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] private AudioClip _basicClick;
    [SerializeField] private AudioClip _use;
    [SerializeField] private AudioClip _useFailed;
    [SerializeField] private AudioClip _validMove;
    [SerializeField] private AudioClip _invalidMove;

    [Header("Volume")]
    [SerializeField] private float _basicClickVolume = 1f;
    [SerializeField] private float _useVolume = 1f;
    [SerializeField] private float _useFailedVolume = 1f;
    [SerializeField] private float _validMoveVolume = 1f;
    [SerializeField] private float _invalidMoveVolume = 1f;

    private InventoryManager _inventoryManager;

    private void OnEnable()
    {
        _inventoryManager = ComponentLocatorService.Components.InventoryManager;

        if (_inventoryManager == null)
        {
            Debug.LogWarning("InventoryFeedback could not find InventoryManager.");
            return;
        }

        _inventoryManager.OnBasicClick += OnBasicClick;
        _inventoryManager.OnUse += OnUse;
        _inventoryManager.OnMove += OnMove;
    }

    private void OnDisable()
    {
        if (_inventoryManager == null)
            return;

        _inventoryManager.OnBasicClick -= OnBasicClick;
        _inventoryManager.OnUse -= OnUse;
        _inventoryManager.OnMove -= OnMove;
    }

    private void OnBasicClick()
    {
        PlaySound(_basicClick, _basicClickVolume);
    }

    private void OnUse(ItemSO item, bool isValid)
    {
        if (!isValid)
        {
            PlaySound(_useFailed, _useFailedVolume);
            return;
        }

        if (item != null && item.UseClip != null)
        {
            PlaySound(item.UseClip, item.UseClipVolume);
            return;
        }

        PlaySound(_use, _useVolume);
    }

    private void OnMove(bool isValid)
    {
        if (isValid)
        {
            PlaySound(_validMove, _validMoveVolume);
        }
        else
        {
            PlaySound(_invalidMove, _invalidMoveVolume);
        }
    }

    private void PlaySound(AudioClip clip, float volume)
    {
        if (clip == null)
            return;

        AudioInfo audioInfo = new AudioInfo(clip, volume);
        ComponentLocatorService.Components.SfxManager.PlaySound(audioInfo, transform.position);
    }
}