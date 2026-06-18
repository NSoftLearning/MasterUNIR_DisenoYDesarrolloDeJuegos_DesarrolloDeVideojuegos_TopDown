using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAttack
{
    public event Action Performed;
    bool CanAttack(List<DamageableTypeSO> attackValidTarget);// List<FoundTargetDTO<IDamageReceiver>> potentialTargets);
    void  PerformAttack();
}
