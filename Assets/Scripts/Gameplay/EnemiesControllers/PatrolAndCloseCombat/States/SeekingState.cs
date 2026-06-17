using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SeekingState<TStateId> : IGenericState<TStateId> where TStateId : Enum
{
    public TStateId StateId { get; }

    private TStateId _handleTargetReached;
    private TStateId _handleTargetLost;
    private CustomCharacterController _customCharacterController;
    private Transform _transform;

    private StateChangeDelegate<TStateId> _stateChangeDelegate;
    private DetectionStatesContext _detectionContext;
    float _searchPersistenceTime;


    List<FoundTargetDTO<IDamageReceiver>> _targetsFound = new ();
    float _willDesistAt = 0;
    public SeekingState (
        TStateId thisStateId,
        TStateId handleTargetReached,
        TStateId handleTargetMissed,
        float searchPersistenceTime,
        CustomCharacterController customCharacterController,
        Transform thisTransform,
        DetectionStatesContext detectionContext,
        StateChangeDelegate <TStateId> stateChangeDelegate)
    {
        StateId = thisStateId;
        _handleTargetReached = handleTargetReached;
        _handleTargetLost = handleTargetMissed;
        _searchPersistenceTime = searchPersistenceTime;
        _customCharacterController = customCharacterController;
        _transform = thisTransform;
        _stateChangeDelegate = stateChangeDelegate;
        _detectionContext = detectionContext;
    }

    public void Enter()
    {
        _willDesistAt = Time.time + _searchPersistenceTime;
        //ChangeStateDueTargetLost();
    }

    public void Exit()
    {

    }

    public void Tick()
    {
        if (ChangeStateDueTargetLost())
            return;
        if (_targetsFound.Count > 0)
            _customCharacterController.SetRawMovement((_targetsFound[0].target.GetPosition() - _transform.position).normalized);
    }

    private bool ChangeStateDueTargetLost()
    {
        _targetsFound.Clear();

        _targetsFound = _detectionContext.targetFinder.FindTargets(_detectionContext.basicTargetFindingQuerySettings,_detectionContext.objectToIgnore, _detectionContext.orientationService.Forward);
        if (_targetsFound.Count > 0)
        {
            _willDesistAt = Time.time + _searchPersistenceTime;
            return false;
        }
        else
        {
            if (Time.time > _willDesistAt)
            {
                _stateChangeDelegate.Invoke(StateId, _handleTargetLost);
                return true;
            }
            return false;
        }
    }
}
