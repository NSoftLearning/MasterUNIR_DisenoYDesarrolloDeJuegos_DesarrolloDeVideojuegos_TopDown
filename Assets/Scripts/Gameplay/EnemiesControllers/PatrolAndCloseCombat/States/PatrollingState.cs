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
       
    }

    public void Exit()
    {
        
    }

    public void Tick()
    {
        List<FoundTargetDTO<IDamageReceiver>> targetsFound =
            _context.targetFinder.FindTargets(_context.GetCurrentQueryData());

        if (_targetSelector.TryGetTargetOfInterest (
            targetsFound,
            out FoundTargetDTO<IDamageReceiver> foundTarget))
        {
            _stateChangeDelegate.Invoke (StateId, _nextStateId);
        }



            
    } 
}
