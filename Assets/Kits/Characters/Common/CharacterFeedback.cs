using UnityEngine;

public class CharacterFeedback : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] AudioClip _walk;
    [SerializeField] AudioClip _collectWeapon;

    [Header("Volume")]
    [SerializeField] float _walkVolume = 0.166f;
    [SerializeField] float _collectWeaponVolume = 0.166f;

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

        if (weaponController != null)
        {
            weaponController.OnNewWeapon += OnNewWeapon;
        }
    }

    int walkIndex = -1;
    private void OnWalking()
    {
        AudioInfo audioInfo = new AudioInfo(_walk, _walkVolume);
        walkIndex = ComponentLocatorService.Components.SfxManager.PlayLoopSound(audioInfo);
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
        ComponentLocatorService.Components.SfxManager.PlaySound(audioInfo);
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
