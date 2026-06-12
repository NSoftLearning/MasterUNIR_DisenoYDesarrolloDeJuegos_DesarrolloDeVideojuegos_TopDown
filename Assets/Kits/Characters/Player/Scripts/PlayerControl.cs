using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] InputActionReference _movement;
    [SerializeField] InputActionReference _attack;

    CustomCharacterController _characterController;
    Vector2 _rawMove;


    private void Awake()
    {
        _characterController = GetComponent<CustomCharacterController>();
    }
     
    private void OnEnable()
    {
        _movement.action.Enable();
        _movement.action.started += Move;
        _movement.action.performed += Move;
        _movement.action.canceled += Move;

        _attack.action.Enable();
    }

    private void Move(InputAction.CallbackContext context)
    {
        _rawMove = context.ReadValue<Vector2>();
        _characterController.SetRawMovement(_rawMove);
    }

    private void OnDisable()
    {
         _movement.action.Disable();
        _movement.action.started -= Move;
        _movement.action.performed -= Move;
        _movement.action.canceled -= Move;
        _attack.action.Disable();
    }


}
