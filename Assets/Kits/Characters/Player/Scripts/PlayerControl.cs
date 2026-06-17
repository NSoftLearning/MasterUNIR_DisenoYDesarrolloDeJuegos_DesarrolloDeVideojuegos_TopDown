using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] InputActionReference _movement;
    [SerializeField] InputActionReference _attack;
    [SerializeField] InputActionReference _interact;
    [SerializeField] InputActionReference _roll;
    [SerializeField] InputActionReference _changeWeapon;

    CustomCharacterController _characterController;
    Vector2 _rawMove;

    IInteractables currentInteractable;
    PlayerWeaponController _weaponController;

    private void Awake()
    {
        _characterController = GetComponent<CustomCharacterController>();
        _weaponController = GetComponent<PlayerWeaponController>();
    }
     
    private void OnEnable()
    {
        _movement.action.Enable();
        _attack.action.Enable();
        _interact.action.Enable();
        _roll.action.Enable();
        _changeWeapon.action.Enable();
        _movement.action.started += Move;
        _movement.action.performed += Move;
        _movement.action.canceled += Move;

        _interact.action.performed += OnInteract;
        _roll.action.performed += OnRoll;
         _attack.action.performed += OnAttack;
        _changeWeapon.action.performed += OnChangeWeapon;
    }

    private void Move(InputAction.CallbackContext context)
    {
        _rawMove = context.ReadValue<Vector2>();
        _characterController.SetRawMovement(_rawMove);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        currentInteractable?.StartInteraction();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        _characterController.Roll();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        _characterController.Attack();
    }

    private void OnChangeWeapon(InputAction.CallbackContext context)
    {
        _weaponController.ChangeNextWeapon();
    }

    private void OnDisable()
    {
        _movement.action.Disable();
        _movement.action.started -= Move;
        _movement.action.performed -= Move;
        _movement.action.canceled -= Move;
        _attack.action.Disable();
        _interact.action.Disable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractables interactable)) //Colision con objeto interactuable; Tienda, NPC, botón, etc.
        {
            currentInteractable = interactable;
            currentInteractable.Select();
        }
        else if (collision.CompareTag("Weapon"))
        {
            _weaponController.NewWeapon(collision.gameObject);

            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractables interactable))
        {
            if (currentInteractable == interactable)
            {
                currentInteractable.Unselect();
                currentInteractable = null;
            }
        }
    }
}
