using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAndCloseCombatEnemyController : MonoBehaviour
{
    ITargetFinder<IDamageReceiver> _targeFinder;
    
    [SerializeField] float detectionRange;
    [SerializeField] List<SideSO> _sidesToSearchFor;

    public void InjectDependencies (ITargetFinder<IDamageReceiver> targetFinder)
    {
        _targeFinder = targetFinder;
    }


}
