using System;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingState<TStateId> : IGenericState<TStateId> where TStateId : Enum
{
    public TStateId StateId { get; }

    TStateId _nextStateId; 
    private StateChangeDelegate<TStateId> _stateChangeDelegate;
    DetectionWithForwardContext<IDamageReceiver, BasicTargetFinderQuerySettings> _context;
         
    public PatrollingState (
        TStateId thisStateId,
        TStateId nextStateId,
        DetectionWithForwardContext<IDamageReceiver, BasicTargetFinderQuerySettings> context,
        StateChangeDelegate<TStateId> stateChangeDelegate
        )
    {
        StateId = thisStateId;
        _nextStateId = nextStateId;
        _context = context;
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
            _context.targetFinder.FindTargets(_context.GetCurrentQueryData(),_context.objectToIgnore);
        if (targetsFound.Count > 0)
            _stateChangeDelegate.Invoke(StateId, _nextStateId);
            
    } 
}
