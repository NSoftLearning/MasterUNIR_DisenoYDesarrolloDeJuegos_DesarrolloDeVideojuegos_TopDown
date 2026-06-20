using System;
using UnityEngine;

public class GettingReadyToRunState<TStateId> : IGenericState<TStateId> where TStateId : Enum
{
    public TStateId StateId { get; }

    private TStateId _nextStateId;
    private float _delay;
    private CustomCharacterController _characterController;
    private StateChangeDelegate<TStateId> _stateChangeDelegate;
    float _willStartRunningAtSecond;
    public GettingReadyToRunState (
        TStateId thisStateId,
        TStateId nextStateId,
        float delay,
        CustomCharacterController characterController,
        StateChangeDelegate<TStateId> stateChangeDelegate)
    {
        StateId = thisStateId;
        _nextStateId = nextStateId;
        _delay = delay;
        _characterController = characterController;
        _stateChangeDelegate = stateChangeDelegate;
    }

    public void Enter()
    {
        _willStartRunningAtSecond = Time.time + _delay;
        _characterController.SetRawMovement(Vector2.zero);
    }

    public void Exit()
    {
        
    }

    public void Tick()
    {
        if (Time.time >  _willStartRunningAtSecond)
        {
            _stateChangeDelegate.Invoke(StateId, _nextStateId);
        }
        
    }
}
