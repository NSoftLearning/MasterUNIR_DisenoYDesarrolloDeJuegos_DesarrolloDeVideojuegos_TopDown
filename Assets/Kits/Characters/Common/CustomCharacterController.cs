using System;
using UnityEngine;
using UnityEngine.Events;

public enum Direction
{
    Down,
    Up,
    Left,
    Right
}

public class CustomCharacterController : MonoBehaviour, IVisible, IOrientationService
{
    [SerializeField] private IVisible.Side side;
    [SerializeField] private float _movementSpeed = 3f;
    [SerializeField] private float _rollSpeed = 4f;
    [SerializeField] private float _rollTime = 1f;
    [SerializeField] private Rigidbody2D _rigidbody;

    private Vector2 _rawMovement;
    private CapsuleCollider2D _capsuleCollider;

    [SerializeField] private Animator _animator;

    [Header("Stamina")]
    [SerializeField] private float _maxStamina = 1f;
    [SerializeField] private float _recoverStaminaSpeedPerSecond = 0.15f;
    [SerializeField] private float _rollStamina = 0.25f;

    public UnityEvent<float, float> onStaminaChanged = new UnityEvent<float, float>();

    private float stamina = 0f;

    private PlayerWeaponController _weaponController;
    private CharacterTemporaryStats _temporaryStats;

    public Vector3 Position => transform.position;
    public Vector3 Forward => _forward;

    private bool walk = false;
    public event Action OnWalking;
    public event Action OnStopWalking;
    public event Action OnRoll;
    public event Action OnFinishRoll;

    private bool isDead = false;
    private bool canMove = true;

    
    private Vector3 _forward;
    private Direction lastDirection;

    private void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _weaponController = GetComponent<PlayerWeaponController>();
        _temporaryStats = GetComponent<CharacterTemporaryStats>();

        stamina = _maxStamina;
        NotifyStaminaChanged();

        if (_weaponController != null)
        {
            _weaponController.FinishedAttack += OnFinishedAttack;
        }
    }

    private void OnDestroy()
    {
        if (_weaponController != null)
        {
            _weaponController.FinishedAttack -= OnFinishedAttack;
        }
    }

    private void Update()
    {
        if (isDead)
            return;

        RecoverStamina();

        if (!canMove)
            return;

        RefreshDirection();

        float moveMultiplier = GetMoveSpeedMultiplier();
        _rigidbody.linearVelocity = _rawMovement * _movementSpeed * moveMultiplier;

        RefreshOrientation();
    }

    public void SetRawMovement(Vector2 rawMove)
    {
        if (isDead)
            return;

        _rawMovement = rawMove;

        if (!canMove)
        {
            StopWalking();
            return;
        }

        ActualizeMoveAnimation();
    }

    private void ActualizeMoveAnimation()
    {
        if (_rawMovement.x != 0 || _rawMovement.y != 0)
        {
            _animator.SetFloat("HorizontalVelocity", _rawMovement.x);
            _animator.SetFloat("VerticalVelocity", _rawMovement.y);
            Walking();
        }
        else
        {
            StopWalking();
        }
    }

    private void Walking()
    {
        if (!walk)
        {
            walk = true;
            _animator.SetBool("Walk", true);
            OnWalking?.Invoke();
        }
    }

    private void StopWalking()
    {
        if (walk)
        {
            walk = false;
            _animator.SetBool("Walk", false);
            OnStopWalking?.Invoke();
        }
    }

    public void Roll()
    {
        if (stamina - _rollStamina <= 0 || !canMove || isDead)
            return;

        _animator.SetTrigger("Roll");

        OnRoll?.Invoke();

        canMove = false;

        float moveMultiplier = GetMoveSpeedMultiplier();
        _rigidbody.linearVelocity = _forward * _rollSpeed * moveMultiplier;

        stamina -= _rollStamina;
        NotifyStaminaChanged();

        Invoke(nameof(FinishedRoll), _rollTime);
    }

    private void FinishedRoll()
    {
        _rigidbody.linearVelocity = Vector3.zero;

        OnFinishRoll?.Invoke();

        canMove = true;

        ActualizeMoveAnimation();
    }

    private void RefreshOrientation()
    {

        if (_rigidbody.linearVelocity.magnitude != 0)
        {
            _forward = _rigidbody.linearVelocity.normalized;
        }
    }

    private void RefreshDirection()
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
        float previousStamina = stamina;

        float newStamina = stamina + _recoverStaminaSpeedPerSecond * Time.deltaTime;

        if (newStamina >= _maxStamina)
        {
            stamina = _maxStamina;
        }
        else
        {
            stamina = newStamina;
        }

        if (previousStamina != stamina)
        {
            NotifyStaminaChanged();
        }
    }

    public void Attack()
    {
        if (isDead)
            return;

        if (!canMove)
            return;

        if (_weaponController == null || !_weaponController.HasWeapon())
            return;

        float neededStamina = _weaponController.GetNeededStamina();
        float newStamina = stamina - neededStamina;

        if (newStamina <= 0)
            return;

        canMove = false;

        stamina = newStamina;
        NotifyStaminaChanged();

        _rigidbody.linearVelocity = Vector2.zero;

        _weaponController.Attack(lastDirection);

        StopWalking();
    }

    private void OnFinishedAttack()
    {
        canMove = true;

        ActualizeMoveAnimation();
    }

    public void Die()
    {
        _rigidbody.linearVelocity = new Vector2(0, -2);
        isDead = true;
        _animator.SetBool("Dead", true);
    }

    private float GetMoveSpeedMultiplier()
    {
        if (_temporaryStats == null)
            return 1f;

        return _temporaryStats.MoveSpeedMultiplier;
    }

    private void NotifyStaminaChanged()
    {
        onStaminaChanged?.Invoke(stamina, _maxStamina);
    }
}