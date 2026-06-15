using System;
using UnityEngine;

public class InitialState<TStateId> : IGenericState<TStateId> where TStateId : Enum
{
    public TStateId StateId { get; }

    private StateChangeDelegate<TStateId> _stateChangeDelegate;
    TStateId _nextStateId;
    public InitialState (
        TStateId thisStateId, 
        TStateId nextStateId, 
        StateChangeDelegate<TStateId> stateChangeDelegate)
    {
        StateId = thisStateId;
        _nextStateId = nextStateId;
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
        
    }
}
