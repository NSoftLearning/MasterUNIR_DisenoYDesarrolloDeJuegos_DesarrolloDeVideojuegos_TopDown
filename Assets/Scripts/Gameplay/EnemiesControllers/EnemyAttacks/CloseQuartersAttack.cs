using System;
using System.Collections.Generic;
using UnityEngine;

public class CloseQuartersAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField]
    float _attackRange;
    [SerializeField]
    float _attackDelay;
    [SerializeField]
    int _attackDamage;
    [SerializeField]
    float _damagePushForce;
    float _attackAvailableAtSecond;
    [SerializeField] private BoxCollider2D _attackBox;

    public event Action Performed;
    ITargetFinder<IDamageReceiver, BoxTargetFinderQuerySettings<IDamageReceiver>> targetFinder;
    IOrientationService _orientationService;
    private IDamageReceiver _selfDamageReceiver;
    public void InjectDependencies (
        ITargetFinder<IDamageReceiver, BoxTargetFinderQuerySettings<IDamageReceiver>> targetFinder,
        IDamageReceiver selfDamageReceiver,
        IOrientationService orientationService)
    {
        this.targetFinder = targetFinder;
        _selfDamageReceiver = selfDamageReceiver;
        _orientationService = orientationService;
    }

    void Start ()
    {
        _attackAvailableAtSecond = Time.time + _attackDelay;
    }

    public void PerformAttack(LayerMask _targetLayers, List<DamageableTypeSO> validDamageables, Vector3 damageOrigin)
    {
        float angle = _attackBox.transform.eulerAngles.z;

        BoxTargetFinderQuerySettings<IDamageReceiver> queryData =
            new BoxTargetFinderQuerySettings<IDamageReceiver>(
                _targetLayers,
                _attackBox.transform.TransformPoint(_attackBox.offset),
                _attackBox.size,
                angle,
                _selfDamageReceiver);

        List<FoundTargetDTO<IDamageReceiver>> targetsFound = targetFinder.FindTargets(queryData);

        DamageDataDTO damageDTO = new DamageDataDTO(_attackDamage, validDamageables, damageOrigin, _damagePushForce);

        foreach (FoundTargetDTO<IDamageReceiver> item in targetsFound)
        {
            item.target.TryToDealDamage(damageDTO);
        }


        _attackAvailableAtSecond = Time.time + _attackDelay;
    }

 

    bool IEnemyAttack.CanAttackSomething(LayerMask _targetLayers, List<DamageableTypeSO> validDamageables)
    {

        if (Time.time < _attackAvailableAtSecond)
            return false;

        float angle = _attackBox.transform.eulerAngles.z;

        BoxTargetFinderQuerySettings<IDamageReceiver> queryData =
            new BoxTargetFinderQuerySettings<IDamageReceiver>(
                _targetLayers,
                _attackBox.transform.TransformPoint(_attackBox.offset),
                _attackBox.size,
                angle, 
                _selfDamageReceiver);

        List<FoundTargetDTO<IDamageReceiver>> targetsFound = targetFinder.FindTargets(queryData);

        foreach (FoundTargetDTO<IDamageReceiver> targetFound in targetsFound)
            if (targetFound.target.DamageIsCompatible(validDamageables))
                return true;

        return false;

    }

    void Update ()
    {
        transform.up = -_orientationService.Forward;
    }
}
