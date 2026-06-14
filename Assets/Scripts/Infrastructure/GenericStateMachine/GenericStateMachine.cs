using System;
using System.Collections.Generic;
using UnityEngine;

public class GenericStateMachine<TStateId> where TStateId : Enum
{

    Dictionary<TStateId, IGenericState<TStateId>> _states = new Dictionary<TStateId, IGenericState<TStateId>>();
    IGenericState<TStateId> _currentState;

    public void InitializeMachine(List<IGenericState<TStateId>> statesSet, TStateId initialState)
    {
        foreach (IGenericState<TStateId> state in statesSet)
        {
            _states.Add(state.StateId, state);
        }

        _currentState = _states[initialState];
    }

    public void Update()
    {
        _currentState.Tick();
    }

    public void ChangeStateTo(TStateId stateId)
    {
        _currentState.Exit();
        _currentState = _states[stateId];
        _currentState.Enter();

        Debug.Log("State chaged to :" + _currentState.StateId);
    }

    public void FromStateToState(TStateId stateRequestingChange, TStateId targetStateId)
    {
        if (!EqualityComparer<TStateId>.Default.Equals(_currentState.StateId, stateRequestingChange))
            return;

        ChangeStateTo(targetStateId);
    }
}
