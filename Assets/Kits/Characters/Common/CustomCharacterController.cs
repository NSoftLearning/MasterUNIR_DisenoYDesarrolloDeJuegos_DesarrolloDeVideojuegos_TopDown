using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.U2D.Animation;
using UnityEngine;

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

    public Vector3 Position => _position;
    public Vector3 Forward => _forward;

    float stamina = 0;
    private void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        stamina = _maxStamina;
    }

    bool canMove = true;
    Vector3 _position;
    Vector3 _forward;
    private void Update()
    {
        RecoverStamina();

        if (!canMove) return;

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
}
 
