using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] GameObject _attackPrefab;
    [SerializeField] float _timeBetweenAttacks = 1f;
    [SerializeField] protected float _neededStamina = 0.2f;
    [SerializeField] bool _destroyAttackOnFinish = true;

    public event Action FinishedAttack;

    public float GetNeededStamina()
    {
        return _neededStamina;
    }

    GameObject actAttack;
    public void Attack(Direction direction, Transform location)
    {
        float x = 0, y = 0;
        switch (direction)
        {
            case Direction.Left: x = -1; break;
            case Direction.Right: x = 1; break;
            case Direction.Up: y = -1; break;
            case Direction.Down: y = 1; break;
        }

        actAttack = Instantiate(_attackPrefab, location);

        Animator anim = actAttack.GetComponent<Animator>();

        anim.SetFloat("HorizontalDirection", x);
        anim.SetFloat("VerticalDirection", y);
        anim.SetTrigger("Attack");

        Invoke(nameof(FinishAttack), _timeBetweenAttacks);
    }

    private void FinishAttack()
    {
        if (_destroyAttackOnFinish) Destroy(actAttack);
        FinishedAttack.Invoke();
    }

    public void AutoDestroy()
    {
        Destroy(gameObject);
    }
}
