using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class CustomCharacterController : MonoBehaviour, IVisible
{
    [SerializeField] IVisible.Side side;
    [SerializeField] float _movementSpeed = 3f;
    [SerializeField] Rigidbody2D _rigidbody;
    
    private Vector2 _rawMovement;
    [SerializeField] Animator _animator;


    private void Update()
    {
        _rigidbody.linearVelocity = _rawMovement * _movementSpeed;
    }

    public void SetRawMovement (Vector2 rawMove)
    {
        _rawMovement = rawMove;
        _animator.SetFloat("HorizontalVelocity", rawMove.x);
        _animator.SetFloat("VerticalVelocity", rawMove.y);
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
 
