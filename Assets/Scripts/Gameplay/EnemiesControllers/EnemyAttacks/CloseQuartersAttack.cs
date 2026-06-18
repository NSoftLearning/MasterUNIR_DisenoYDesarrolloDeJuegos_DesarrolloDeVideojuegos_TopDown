using System;
using System.Collections.Generic;
using UnityEngine;

public class CloseQuartersAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField]
    float _attackRange;
    [SerializeField]
    float _attackDelay;
    float _attackAvailableAtSecond;

    public event Action Performed;

    void Start ()
    {
        _attackAvailableAtSecond = Time.time + _attackDelay;
    }

    public void PerformAttack()
    {
        throw new System.NotImplementedException();
    }

    public bool CanAttack(List<DamageableTypeSO> attackValidTarget)
    {
        return false;
        if (Time.time < _attackAvailableAtSecond)
            return false;

        return true;
    }
}
