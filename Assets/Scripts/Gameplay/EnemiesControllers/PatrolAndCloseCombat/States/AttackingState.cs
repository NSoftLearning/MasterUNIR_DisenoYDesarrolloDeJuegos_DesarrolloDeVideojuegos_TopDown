using System;
using UnityEngine;

public class AttackingStatee <TStateId> : IGenericState<TStateId> where TStateId : Enum
{
    public TStateId StateId { get; }

    private TStateId _nextStateId;
    private StateChangeDelegate<TStateId> _stateChangeDelegate;
    private TStateId _handleAttckPerformed;
    IEnemyAttack _enemyAttack;    

    public AttackingStatee (
        TStateId thisStateId,
        TStateId nextStateId,
        StateChangeDelegate<TStateId> stateChangeDelegate)
    {
        StateId = thisStateId;
        _nextStateId = nextStateId;
        _stateChangeDelegate = stateChangeDelegate;
    }

    public void Enter()
    {
        _enemyAttack.Performed += TeardownAndStateChange;
        _enemyAttack.PerformAttack();
    }

    public void Exit()
    {
        _enemyAttack.Performed -= TeardownAndStateChange;
    }

    public void Tick()
    {

    }

    private void TeardownAndStateChange()
    {
        _stateChangeDelegate.Invoke(StateId, _nextStateId);
    }
}
