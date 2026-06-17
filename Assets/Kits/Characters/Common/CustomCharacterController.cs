using System;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.U2D.Animation;
using UnityEngine;
using static UnityEditor.PlayerSettings.SplashScreen;

public enum Direction
{
    Down,
    Up, 
    Left, 
    Right
}

public class CustomCharacterController : MonoBehaviour, IVisible, IOrientationService
{
    [SerializeField] IVisible.Side side;
    [SerializeField] float _movementSpeed = 3f;
    [SerializeField] float _rollSpeed = 4f;
    [SerializeField] float _rollTime = 1f;
    [SerializeField] Rigidbody2D _rigidbody;

    private Vector2 _rawMovement;
    private CapsuleCollider2D _capsuleCollider;
    [SerializeField] Animator _animator;

    [Header("Stamina")]
    [SerializeField] float _maxStamina = 1f;
    [SerializeField] float _recoverStaminaSpeedPerSecond = 0.15f;
    [SerializeField] float _rollStamina = 0.25f;

    private PlayerWeaponController _weaponController;

    public Vector3 Position => _position;
    public Vector3 Forward => _forward;

    float stamina = 0;
    private void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _weaponController = GetComponent<PlayerWeaponController>();

        stamina = _maxStamina;

        if (_weaponController != null)
        {
            _weaponController.FinishedAttack += OnFinishedAttack;
        }
    }

    bool canMove = true;
    Vector3 _position;
    Vector3 _forward;
    Direction lastDirection;
    private void Update()
    {
        RecoverStamina();

        if (!canMove) return;

        RefreshDirection();
        _rigidbody.linearVelocity = _rawMovement * _movementSpeed;
        RefreshOrientation();
    }

    public void SetRawMovement(Vector2 rawMove)
    {
        _rawMovement = rawMove;

        if (!canMove) return;
        
        _animator.SetFloat("HorizontalVelocity", rawMove.x);
        _animator.SetFloat("VerticalVelocity", rawMove.y);
    }

    public void Roll()
    {
        if (stamina - _rollStamina <= 0 || !canMove) return;

        _animator.SetTrigger("Roll");
        _capsuleCollider.enabled = false;
        canMove = false;
        _rigidbody.linearVelocity = _rawMovement * _rollSpeed;
        stamina -= _rollStamina;

        Invoke(nameof(FinishedRoll), _rollTime);
    }

    private void FinishedRoll()
    {
        _rigidbody.linearVelocity = Vector3.zero;
        _capsuleCollider.enabled = true;
        canMove = true;
    }


    void RefreshOrientation()
    {
        _position = transform.position;
        if (_rigidbody.linearVelocity.magnitude != 0)
            _forward = _rigidbody.linearVelocity.normalized;
    }

    void RefreshDirection()
    {
        if (_rawMovement != Vector2.zero)
        {
            if (Mathf.Abs(_rawMovement.x) > Mathf.Abs(_rawMovement.y))
            {
                lastDirection = (_rawMovement.x > 0) ? Direction.Right : Direction.Left;
            }
            else
            {
                lastDirection = (_rawMovement.y > 0) ? Direction.Up : Direction.Down;
            }
        }
    }

    public IVisible.Side GetSide()
    {
        return side;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    private void RecoverStamina()
    {
        float newStamina = stamina + _recoverStaminaSpeedPerSecond * Time.deltaTime;
        if (newStamina >= _maxStamina) stamina = _maxStamina;
        else stamina = newStamina;
    }

    public void Attack()
    {
        if (!canMove || !_weaponController.HasWeapon()) return;
        
        float neededStamina = _weaponController.GetNeededStamina();
        float newStamina = stamina - neededStamina;

        if (newStamina <= 0) return;

        canMove = false;
        stamina = newStamina;
        _rigidbody.linearVelocity = Vector2.zero;
        
        _weaponController.Attack(lastDirection);
    }

    private void OnFinishedAttack()
    {
        canMove = true;
    }
}
 
