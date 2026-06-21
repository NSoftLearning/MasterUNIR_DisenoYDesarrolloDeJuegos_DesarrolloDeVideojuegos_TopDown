using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAttack
{
    public event Action Performed;
    CanAttackStatus CanAttackSomething(LayerMask _targetLayers, List<DamageableTypeSO> _validDamageables);//List<DamageableTypeSO> attackValidTarget);// List<FoundTargetDTO<IDamageReceiver>> potentialTargets);
    void PerformAttack(LayerMask _targetLayers, List<DamageableTypeSO> validDamageables, Vector3 damageOrigin);
}
