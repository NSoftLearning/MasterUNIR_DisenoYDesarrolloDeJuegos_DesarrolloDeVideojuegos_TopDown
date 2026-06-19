using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float linearVelocity = 2f;

    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Direction dir)
    {
        float x = 0, y = 0;
        switch (dir)
        {
            case Direction.Up: y = 1; break;
            case Direction.Down: y = -1; break;
            case Direction.Left: x = -1; break;
            case Direction.Right: x = 1; break;
        }

        rb.linearVelocityX = x * linearVelocity;
        rb.linearVelocityY = y * linearVelocity;


        Animator anim = GetComponent<Animator>();

        anim.SetFloat("HorizontalDirection", x);
        anim.SetFloat("VerticalDirection", -y);
    }

    public void FinishMovement()
    {
        // Particulas de explosion
        Destroy(gameObject);
    }
}
