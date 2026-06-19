using UnityEngine;

public class ProjectileCollisionManager : MonoBehaviour
{
    HitCollider hitCollider;
    Projectile projectile;

    private void Awake()
    {
        hitCollider = GetComponent<HitCollider>();
        projectile = GetComponent<Projectile>();
    }

    private void OnEnable()
    {
        hitCollider.OnHit += OnHit;
    }

    private void OnHit()
    {
        projectile?.FinishMovement();
    }

    private void OnDisable()
    {
        hitCollider.OnHit -= OnHit;
    }
}
