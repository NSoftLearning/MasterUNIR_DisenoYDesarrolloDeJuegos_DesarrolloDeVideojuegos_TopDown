
using System;
using System.Collections.Generic;
using UnityEngine;

public class SeekingState<TStateId> : IGenericState<TStateId> where TStateId : Enum
{
    public TStateId StateId { get; }

    private TStateId _handleTargetReached;
    private TStateId _handleTargetLost;
    private Transform _transform;

    private StateChangeDelegate<TStateId> _stateChangeDelegate;
    private DamageReceiverTargetSelector _targetSelector;
    DetectionWithForwardAndIgnoreContext<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> _detectionContext;
    private IEnemyAttack _enemyAttack;
    float _searchPersistenceTime;
    List<DamageableTypeSO> _damageableTypesOfInterest;
    
    List<FoundTargetDTO<IDamageReceiver>> _currentTargetsOfInterest = new ();
    float _willDesistAt = 0;
    float _thisStateDetectionRange;
    Vector3 _targetLastKnownPosition;
    bool _hasTargetLastKnownPosition;
    // DistanceAndLosTargetFinderQuerySettings<IDamageReceiver> thisStateQueryOverride;
    public SeekingState(
        TStateId thisStateId,
        TStateId handleTargetReached,
        TStateId handleTargetMissed,
        float searchPersistenceTime,
        Transform thisTransform,
        DetectionWithForwardAndIgnoreContext<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> detectionContext,
        DamageReceiverTargetSelector targetSelector,
        IEnemyAttack enemyAttack,
        List<DamageableTypeSO> damageableTypesOfInterest,
        float alertRange,
    StateChangeDelegate <TStateId> stateChangeDelegate)
    {
        StateId = thisStateId;
        _handleTargetReached = handleTargetReached;
        _handleTargetLost = handleTargetMissed;
        _searchPersistenceTime = searchPersistenceTime;
        _transform = thisTransform;
        _stateChangeDelegate = stateChangeDelegate;
        _targetSelector = targetSelector; 
        _detectionContext = detectionContext;
        _enemyAttack = enemyAttack;
        _damageableTypesOfInterest = damageableTypesOfInterest;
        _thisStateDetectionRange = alertRange;
   //     thisStateQueryOverride = _detectionContext.GetCurrentQueryData();
  //      thisStateQueryOverride.range = alertRange;
    }

    public void Enter()
    {
        _willDesistAt = Time.time + _searchPersistenceTime;

    }

    public void Exit()
    {

    }

    public void Tick()
    {
        RefreshDetectedTargets();

        if (_enemyAttack.CanAttackSomething(_detectionContext.GetCurrentQueryData().layersToSearch, _damageableTypesOfInterest))
        { // _targetSelector.DamageableTypes))// _currentTargetsOfInterest))
            _stateChangeDelegate.Invoke(StateId, _handleTargetReached);
            return;
        }


        if (_currentTargetsOfInterest.Count > 0)
        {
            _targetLastKnownPosition = _currentTargetsOfInterest[0].target.GetPosition();
            _hasTargetLastKnownPosition = true;

            _detectionContext.customCharacterController.SetRawMovement(
                (_targetLastKnownPosition - _transform.position).normalized);

            _willDesistAt = Time.time + _searchPersistenceTime;
            return;
        }
        if (_currentTargetsOfInterest.Count == 0)
        {
            if (Time.time > _willDesistAt)
            {
                _stateChangeDelegate.Invoke(StateId, _handleTargetLost);
                return;
            }

            if (!_hasTargetLastKnownPosition)
            {
                _detectionContext.customCharacterController.SetRawMovement(Vector2.zero);
                return;
            }

            if (Vector3.Distance(_transform.position, _targetLastKnownPosition) < .1f)
            {
                _detectionContext.customCharacterController.SetRawMovement(Vector2.zero);
                return;
            }

            bool directionFound = _detectionContext.directionFindingService.TryGetDirection(
                _detectionContext.orientationService.Position,
                _targetLastKnownPosition,
                out Vector3 targetDirection,
                out Vector3 targetCornerPosition);

            if (!directionFound)
            {
                _detectionContext.customCharacterController.SetRawMovement(Vector2.zero);
                return;
            }

            _detectionContext.customCharacterController.SetRawMovement(targetDirection.normalized);
            return;
        }



    }

    private void RefreshDetectedTargets()
    {
        _currentTargetsOfInterest.Clear();

        DistanceAndLosTargetFinderQuerySettings<IDamageReceiver> overridedRangeQuery =
        _detectionContext.GetCurrentQueryData();

        overridedRangeQuery.range = _thisStateDetectionRange;

        List<FoundTargetDTO<IDamageReceiver>> detectedTargets =
            _detectionContext.targetFinder.FindTargets(overridedRangeQuery);

        if (_targetSelector.TryGetTargetOfInterest(
                detectedTargets,
                out FoundTargetDTO<IDamageReceiver> targetOfInterest))
        {
            
            _currentTargetsOfInterest.Add(targetOfInterest);
        }
    }
}
