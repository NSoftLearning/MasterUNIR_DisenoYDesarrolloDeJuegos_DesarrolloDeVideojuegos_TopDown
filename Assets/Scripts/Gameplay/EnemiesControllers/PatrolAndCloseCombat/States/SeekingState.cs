
using System;
using System.Collections.Generic;
using UnityEngine;

public class SeekingState<TStateId> : IGenericState<TStateId> where TStateId : Enum
{
    public TStateId StateId { get; }

    private TStateId _handleTargetReached;
    private TStateId _handleLastKnownPositionReached;

    private Transform _transform;

    private StateChangeDelegate<TStateId> _stateChangeDelegate;
    private DamageReceiverTargetSelector _targetSelector;
    DetectionWithForwardAndIgnoreContext<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> _detectionContext;
    private IEnemyAttack _enemyAttack;
    float _searchPersistenceTime;
    List<DamageableTypeSO> _damageableTypesOfInterest;
    
    List<FoundTargetDTO<IDamageReceiver>> _currentTargetsOfInterest = new ();
    float _thisStateDetectionRange;
    Vector3 _targetLastKnownPosition;
    bool _hasTargetLastKnownPosition;

    public SeekingState(
        TStateId thisStateId,
        TStateId handleTargetReached,
        TStateId handleLastKnownPositionReached,
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
        _handleLastKnownPositionReached = handleLastKnownPositionReached;
        _transform = thisTransform;
        _stateChangeDelegate = stateChangeDelegate;
        _targetSelector = targetSelector; 
        _detectionContext = detectionContext;
        _enemyAttack = enemyAttack;
        _damageableTypesOfInterest = damageableTypesOfInterest;
        _thisStateDetectionRange = alertRange;
    }

    public void Enter()
    {
    }

    public void Exit()
    {

    }

    public void Tick()
    {
        RefreshDetectedTargets();

        var canAttackStatus = _enemyAttack.CanAttackSomething(_detectionContext.GetCurrentQueryData().layersToSearch, _damageableTypesOfInterest);

        if (canAttackStatus.canAttack
            && canAttackStatus.isInRange)
        { 
            _stateChangeDelegate.Invoke(StateId, _handleTargetReached);
            return;
        }
        if (_currentTargetsOfInterest.Count > 0
            && canAttackStatus.isInRange)
        {
            _detectionContext.customCharacterController.SetRawMovement(Vector2.zero);
            return;
        }

        if (_currentTargetsOfInterest.Count > 0)
        {
            _targetLastKnownPosition = _currentTargetsOfInterest[0].target.GetPosition();
            _hasTargetLastKnownPosition = true;

            _detectionContext.customCharacterController.SetRawMovement(
                (_targetLastKnownPosition - _transform.position).normalized);

            return;
        }
        if (_currentTargetsOfInterest.Count == 0)
        {


            if (!_hasTargetLastKnownPosition)
            {
                _detectionContext.customCharacterController.SetRawMovement(Vector2.zero);
                return;
            }

            if (Vector3.Distance(_transform.position, _targetLastKnownPosition) < .1f)
            {
                _stateChangeDelegate.Invoke(StateId, _handleLastKnownPositionReached);
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
