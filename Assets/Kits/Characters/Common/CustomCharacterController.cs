using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class CustomCharacterController : MonoBehaviour, IVisible, IOrientationService
{
    [SerializeField] IVisible.Side side;
    [SerializeField] float _movementSpeed = 3f;
    [SerializeField] Rigidbody2D _rigidbody;
    
    private Vector2 _rawMovement;
    [SerializeField] Animator _animator;

    public Vector3 Position => _position;
    public Vector3 Forward => _forward;


    Vector3 _position;
    Vector3 _forward;
    private void Update()
    {
        _rigidbody.linearVelocity = _rawMovement * _movementSpeed;
        RefreshOrientation();
    }

    public void SetRawMovement (Vector2 rawMove)
    {
        _rawMovement = rawMove;
        _animator.SetFloat("HorizontalVelocity", rawMove.x);
        _animator.SetFloat("VerticalVelocity", rawMove.y);
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
}
 
