using UnityEngine;

public class CharacterFeedback : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] AudioClip _walk;
    [SerializeField] AudioClip _collectWeapon;
    [SerializeField] AudioClip _roll;

    [Header("Volume")]
    [SerializeField] float _walkVolume = 0.3f;
    [SerializeField] float _rollVolume = 0.3f;
    [SerializeField] float _collectWeaponVolume = 0.3f;

    CustomCharacterController controller;
    PlayerWeaponController weaponController;
    private void Awake()
    {
        controller = GetComponent<CustomCharacterController>();
        weaponController = GetComponent<PlayerWeaponController>();
    }

    private void OnEnable()
    {
        controller.OnWalking += OnWalking;
        controller.OnStopWalking += OnStopWalking;
        controller.OnRoll += OnRoll;

        if (weaponController != null)
        {
            weaponController.OnNewWeapon += OnNewWeapon;
        }
    }

    int walkIndex = -1;
    private void OnWalking()
    {
        AudioInfo audioInfo = new AudioInfo(_walk, _walkVolume);
        walkIndex = ComponentLocatorService.Components.SfxManager.PlayLoopSound(audioInfo, transform);
    }

    private void OnRoll()
    {
        AudioInfo attackAudio = new AudioInfo(_roll, _rollVolume);
        ComponentLocatorService.Components.SfxManager.PlaySound(attackAudio, transform.position);
    }

    private void OnStopWalking()
    {
        if (walkIndex != -1)
        {
            ComponentLocatorService.Components.SfxManager.StopLoopSound(walkIndex);
            walkIndex = -1;
        }
    }

    private void OnNewWeapon()
    {
        AudioInfo audioInfo = new AudioInfo(_collectWeapon, _collectWeaponVolume);
        ComponentLocatorService.Components.SfxManager.PlaySound(audioInfo, transform.position);
    }

    private void OnDisable()
    {
        controller.OnWalking -= OnWalking;
        controller.OnStopWalking -= OnStopWalking;

        if (weaponController != null)
        {
            weaponController.OnNewWeapon -= OnNewWeapon;
        }
    }
}
