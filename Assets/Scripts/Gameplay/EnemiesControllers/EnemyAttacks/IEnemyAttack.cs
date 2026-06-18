using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAttack
{
    bool CanAttack(List<IDamageReceiver> candidatesForAttack);
    void  PerformAttack();
}
