using UnityEngine;

public class DoorFeedback : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] AudioClip _openDoor;
    [SerializeField] AudioClip _closeDoor;

    [Header("Volume")]
    [SerializeField] float _openDoorVolume;
    [SerializeField] float _closeDoorVolume;

    Door door;
    private void Awake()
    {
        door = GetComponent<Door>();
    }

    private void OnEnable()
    {
        door.DoorOpen += OpenDoor;
        door.DoorClose += CloseDoor;
    }

    private void OpenDoor()
    {
        PlaySound(_openDoor, _openDoorVolume);
    }

    private void CloseDoor()
    {
        PlaySound(_closeDoor, _closeDoorVolume);
    }

    private void PlaySound(AudioClip clip, float vol)
    {
        AudioInfo audioInfo = new AudioInfo(clip, vol);
        ComponentLocatorService.Components.SfxManager.PlaySound(audioInfo);
    }

    private void OnDisable()
    {
        door.DoorOpen -= OpenDoor;
        door.DoorClose -= CloseDoor;
    }
}
