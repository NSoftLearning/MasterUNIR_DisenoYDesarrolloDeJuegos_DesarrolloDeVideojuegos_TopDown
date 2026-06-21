using UnityEngine;

public class WeaponFeedback : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] AudioClip _attack;

    [Header("Volume")]
    [SerializeField] float _attackVolume;

    Weapon weapon;
    private void Awake()
    {
        weapon = GetComponent<Weapon>();
    }

    private void OnEnable()
    {
        weapon.OnAttack += OnAttack;
    }

    private void OnAttack()
    {
        Debug.Log("HOLA");
        AudioInfo attackAudio = new AudioInfo(_attack, _attackVolume);
        ComponentLocatorService.Components.SfxManager.PlaySound(attackAudio);
    }

    private void OnDisable()
    {
        weapon.OnAttack -= OnAttack;
    }
}
