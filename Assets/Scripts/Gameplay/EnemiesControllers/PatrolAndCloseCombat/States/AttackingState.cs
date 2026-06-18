using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackingStatee <TStateId> : IGenericState<TStateId> where TStateId : Enum
{
    public TStateId StateId { get; }

    private TStateId _nextStateId;
    private StateChangeDelegate<TStateId> _stateChangeDelegate;
    private TStateId _handleAttckPerformed;
    IEnemyAttack _enemyAttack;
    LayerMask _layerstToSearchForTarget;
    List<DamageableTypeSO> _damageableTypesOfInterest;
    Transform _damageOriginTransform;
    public AttackingStatee (
        TStateId thisStateId,
        TStateId nextStateId,
        IEnemyAttack enemyAttack,
        LayerMask layersToSearchForTarget,
        List<DamageableTypeSO> damageableTypeSo,
        Transform damageOriginTransform,
        StateChangeDelegate<TStateId> stateChangeDelegate)
    {
        StateId = thisStateId;
        _nextStateId = nextStateId;
        _stateChangeDelegate = stateChangeDelegate;
        _enemyAttack = enemyAttack;
        _layerstToSearchForTarget = layersToSearchForTarget;
        _damageableTypesOfInterest = damageableTypeSo;
        _damageOriginTransform = damageOriginTransform;
    }

    public void Enter()
    {
        _enemyAttack.Performed += TeardownAndStateChange;
        _enemyAttack.PerformAttack(_layerstToSearchForTarget, _damageableTypesOfInterest, _damageOriginTransform.position);
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
