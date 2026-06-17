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
            case Direction.Up: y = linearVelocity; break;
            case Direction.Down: y = -linearVelocity; break;
            case Direction.Left: x = -linearVelocity; break;
            case Direction.Right: x = linearVelocity; break;
        }

        rb.linearVelocityX = x;
        rb.linearVelocityY = y;
    }
}
