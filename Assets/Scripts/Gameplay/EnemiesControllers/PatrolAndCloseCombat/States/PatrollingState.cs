using System;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingState<TStateId> : IGenericState<TStateId> where TStateId : Enum
{
    public TStateId StateId { get; }

    TStateId _nextStateId; 
    private StateChangeDelegate<TStateId> _stateChangeDelegate;
    private ITargetFinder<IDamageReceiver, BasicTargetFinderQuerySettings> _targetFinder;
    private IOrientationService _orientationService;
    private List <IDamageReceiver> _selfDamageRecevier;
    private BasicTargetFinderQuerySettings _targetFinderDTO;
     
    public PatrollingState (
        TStateId thisStateId,
        TStateId nextStateId,
        BasicTargetFinderQuerySettings targetFinderQuerySettings,
        ITargetFinder <IDamageReceiver, BasicTargetFinderQuerySettings> targetFidner, 
        IDamageReceiver selfDamageReceiver,
        IOrientationService orientationService,
        StateChangeDelegate<TStateId> stateChangeDelegate
        )
    {
        StateId = thisStateId;
        _nextStateId = nextStateId;
        _targetFinderDTO = targetFinderQuerySettings;
        _stateChangeDelegate = stateChangeDelegate;
        _targetFinder = targetFidner;
        _orientationService = orientationService;
        _selfDamageRecevier = new List<IDamageReceiver> { selfDamageReceiver };
    }

    public void Enter()
    {
       
    }

    public void Exit()
    {
        
    }

    public void Tick()
    {
        List<FoundTargetDTO<IDamageReceiver>> targetsFound = _targetFinder.FindTargets(_targetFinderDTO, _selfDamageRecevier, _orientationService.Forward);
        if (targetsFound.Count > 0)
            _stateChangeDelegate.Invoke(StateId, _nextStateId);
            
    } 
}
