using System.Collections.Generic;
using UnityEngine;

public class CloseQuartersAttack :MonoBehaviour,  IEnemyAttack
{
    [SerializeField]
    float _attackRange;
    [SerializeField]
    float _attackDelay;
    float _attackAvailableAtSecond;


    void Start ()
    {
        _attackAvailableAtSecond = Time.time + _attackDelay;
    }
    public bool CanAttack(List<IDamageReceiver> candidatesForAttack)
    {
        if (Time.time < _attackAvailableAtSecond)
            return false;
        //aqui ver si al menos un candidate esta in range Y si el ataque esta listo
        throw new System.NotImplementedException();
    }

    public void PerformAttack()
    {
        throw new System.NotImplementedException();
    }
}
