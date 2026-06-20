using System;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingState<TStateId> : IGenericState<TStateId> where TStateId : Enum
{
    public TStateId StateId { get; }

    TStateId _nextStateId; 
    private StateChangeDelegate<TStateId> _stateChangeDelegate;
    DetectionWithForwardAndIgnoreContext<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> _context;
    private List<DamageableTypeSO> _damageableTypesOfInterest;
    private DamageReceiverTargetSelector _targetSelector;
    PatrolWaypoint currentPatrolWaypoint;
    bool goingForward;
    Vector3 targetCornerPosition;
    Vector3 targetCornerDirection;
    public PatrollingState (
        TStateId thisStateId,
        TStateId nextStateId,
        DetectionWithForwardAndIgnoreContext<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> context,
        DamageReceiverTargetSelector targetSelector,
        List<DamageableTypeSO> damageableTypesOfInterest,        
        StateChangeDelegate<TStateId> stateChangeDelegate
        )
    {
        StateId = thisStateId;
        _nextStateId = nextStateId;
        _context = context;
        _damageableTypesOfInterest = damageableTypesOfInterest;
        _targetSelector = targetSelector;
        _stateChangeDelegate = stateChangeDelegate;
    }

    public void Enter()
    {
        currentPatrolWaypoint = _context.patrolRoute.GetClosestWaypoint(_context.orientationService.Position);
        _context.directionFindingService.TryGetDirection(_context.orientationService.Position, currentPatrolWaypoint.transform.position, out targetCornerDirection, out targetCornerPosition);
       
    }

    public void Exit()
    {
        
    }

    public void Tick()
    {
        RefreshMovementDirection();
        CheckForCurrentTargetCornerChange();
        List<FoundTargetDTO<IDamageReceiver>> targetsFound =
            _context.targetFinder.FindTargets(_context.GetCurrentQueryData());

        if (_targetSelector.TryGetTargetOfInterest (
            targetsFound,
            out FoundTargetDTO<IDamageReceiver> foundTarget))
        {
            _stateChangeDelegate.Invoke (StateId, _nextStateId);
        }            
    }

    private void CheckForCurrentTargetCornerChange()
    {
        if (Vector3.Distance(currentPatrolWaypoint.transform.position, _context.orientationService.Position) < .1f)
        {
            currentPatrolWaypoint = _context.patrolRoute.GetNextWaypoint(currentPatrolWaypoint, ref goingForward);
            _context.directionFindingService.TryGetDirection(_context.orientationService.Position, currentPatrolWaypoint.transform.position, out targetCornerDirection, out targetCornerPosition);
            return;
        }

        if (Vector3.Distance (targetCornerPosition, _context.orientationService.Position) < .1f)
        {
            _context.directionFindingService.TryGetDirection(_context.orientationService.Position, currentPatrolWaypoint.transform.position, out targetCornerDirection, out targetCornerPosition);
        }

    }

    private void RefreshMovementDirection ()
    {
        _context.customCharacterController.SetRawMovement((targetCornerPosition - _context.orientationService.Position).normalized);
    }

   
}
