using System;
using System.Collections.Generic;
using UnityEngine;

public class GuardingLastKnownPosition<TStateId> : IGenericState<TStateId> where TStateId : Enum
{
    public TStateId StateId { get; }

    private TStateId _handleEndOfGuard;
    private TStateId _handleTargetAcquiredAgain;
    private DetectionWithForwardAndIgnoreContext<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> _context;
    private float _guardPersistenceTime;
    float willDesistAt;
    List<FoundTargetDTO<IDamageReceiver>> _currentTargetsOfInterest = new();
    private float _thisStateDetectionRange;
    private DamageReceiverTargetSelector _targetSelector;
    private StateChangeDelegate<TStateId> _stateChangeDelegate;

    public GuardingLastKnownPosition (
        TStateId thisStateId,
        TStateId handleEndOfGuard,
        TStateId handleTargetAcquiredAgain,
        float guardPersistenceTime,
        DamageReceiverTargetSelector targetSelector,
         DetectionWithForwardAndIgnoreContext<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> context,
         float thisStateDetectionRange,
         StateChangeDelegate<TStateId> stateChangeDelegate)
    {
        StateId = thisStateId;
        _handleEndOfGuard = handleEndOfGuard;
        _context = context;
        _guardPersistenceTime = guardPersistenceTime;
        _targetSelector = targetSelector;
        _stateChangeDelegate = stateChangeDelegate;
        _handleTargetAcquiredAgain = handleTargetAcquiredAgain;
        _thisStateDetectionRange = thisStateDetectionRange;
    }
    public void Enter()
    {
        _context.customCharacterController.SetRawMovement(Vector2.zero);
        willDesistAt = Time.time + _guardPersistenceTime;
        
    }

    public void Exit()
    {
        
    }

    public void Tick()
    {
        RefreshDetectedTargets();
    }

    private void RefreshDetectedTargets()
    {
        _currentTargetsOfInterest.Clear();

        DistanceAndLosTargetFinderQuerySettings<IDamageReceiver> overridedRangeQuery =
        _context.GetCurrentQueryData();

        overridedRangeQuery.range = _thisStateDetectionRange;
        overridedRangeQuery.halfFieldOfView = 180;

        List<FoundTargetDTO<IDamageReceiver>> detectedTargets =
            _context.targetFinder.FindTargets(overridedRangeQuery);

        if (_targetSelector.TryGetTargetOfInterest(
                detectedTargets,
                out FoundTargetDTO<IDamageReceiver> targetOfInterest))
        {

            _context.customCharacterController.SetRawMovement(detectedTargets[0].position - _context.orientationService.Position);
            _stateChangeDelegate.Invoke(StateId, _handleTargetAcquiredAgain);
        }
    }
}
