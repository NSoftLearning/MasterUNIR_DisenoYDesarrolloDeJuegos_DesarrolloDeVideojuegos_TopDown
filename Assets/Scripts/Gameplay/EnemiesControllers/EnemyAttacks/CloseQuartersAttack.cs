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
    [SerializeField] Animator _attackAnimator;
    public event Action Performed;
    ITargetFinder<IDamageReceiver, BoxTargetFinderQuerySettings<IDamageReceiver>> targetFinder;
    IOrientationService _orientationService;
    private IDamageReceiver _selfDamageReceiver;
    AnimationEventsAdapter _animationEventsAdapter;

    LayerMask _targetLayers;
    List<DamageableTypeSO> _validDamageables;
    Vector3 _attackOrigin;
    public void InjectDependencies (
        ITargetFinder<IDamageReceiver, BoxTargetFinderQuerySettings<IDamageReceiver>> targetFinder,
        IDamageReceiver selfDamageReceiver,
        IOrientationService orientationService,
        AnimationEventsAdapter animationEventsAdapter)
    {
        this.targetFinder = targetFinder;
        _selfDamageReceiver = selfDamageReceiver;
        _orientationService = orientationService;
        _animationEventsAdapter = animationEventsAdapter;
    }

    void Start ()
    {
        _attackAvailableAtSecond = Time.time + _attackDelay;
    }

    public void PerformAttack(LayerMask targetLayers, List<DamageableTypeSO> validDamageables, Vector3 damageOrigin)
    {
        _targetLayers = targetLayers; 
        _validDamageables = validDamageables;
        _attackOrigin = damageOrigin;

        _attackAnimator.SetTrigger("StartAttack");






    }

 

    CanAttackStatus IEnemyAttack.CanAttackSomething(LayerMask _targetLayers, List<DamageableTypeSO> validDamageables)
    {

        CanAttackStatus returnValue;

        returnValue.isInRange = false;
        if (Time.time < _attackAvailableAtSecond)
            returnValue.canAttack = false;
        else
            returnValue.canAttack = true;


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
                returnValue.isInRange = true;

        

        return returnValue;

    }

    void Update ()
    {
        transform.up = -_orientationService.Forward;
    }


    public void DealDamage ()
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

        DamageDataDTO damageDTO = new DamageDataDTO(_attackDamage, _validDamageables, _attackOrigin, _damagePushForce);

        foreach (FoundTargetDTO<IDamageReceiver> item in targetsFound)
        {
            item.target.TryToDealDamage(damageDTO);
        }


        _attackAvailableAtSecond = Time.time + _attackDelay;

    }

    public void PerformEndOfAttackActions ()
    {
        Performed?.Invoke();
    }

    void OnEnable ()
    {
        _animationEventsAdapter.AnimationReachedPointOfInterest += DealDamage;
        _animationEventsAdapter.AnimationEnded += PerformEndOfAttackActions;
    }

    void OnDisable ()
    {
        _animationEventsAdapter.AnimationReachedPointOfInterest -= DealDamage;
        _animationEventsAdapter.AnimationEnded -= PerformEndOfAttackActions;
    }
}
