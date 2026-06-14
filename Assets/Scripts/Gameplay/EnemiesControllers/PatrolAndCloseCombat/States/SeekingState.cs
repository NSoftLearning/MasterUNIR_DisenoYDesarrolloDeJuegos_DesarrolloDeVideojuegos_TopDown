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
    private BasicTargetFinderQuerySettings _basicTargetFinderQuerySettings;
    private ITargetFinder<IDamageReceiver, BasicTargetFinderQuerySettings> _targetFinder;
    private List<IDamageReceiver> _selfDamageReceiver;
    private CustomCharacterController _customCharacterController;
    private Transform _transform;
    private IOrientationService _orientationService;
    private StateChangeDelegate<TStateId> _stateChangeDelegate;
    float _searchPersistenceTime;


    List<FoundTargetDTO<IDamageReceiver>> _targetsFound = new ();
    float _willDesistAt = 0;
    public SeekingState (
        TStateId thisStateId,
        TStateId handleTargetReached,
        TStateId handleTargetMissed,
        float searchPersistenceTime,
        BasicTargetFinderQuerySettings basicTargetFinderQuerySettings,
        ITargetFinder <IDamageReceiver, BasicTargetFinderQuerySettings> targetFinder,
        IDamageReceiver selfDamageReceiver,
        CustomCharacterController customCharacterController,
        Transform thisTransform,
        IOrientationService orientationService,
        StateChangeDelegate <TStateId> stateChangeDelegate)
    {
        StateId = thisStateId;
        _handleTargetReached = handleTargetReached;
        _handleTargetLost = handleTargetMissed;
        _searchPersistenceTime = searchPersistenceTime;
        _basicTargetFinderQuerySettings = basicTargetFinderQuerySettings;
        _targetFinder = targetFinder;
        _selfDamageReceiver = new List<IDamageReceiver> { selfDamageReceiver };
        _customCharacterController = customCharacterController;
        _transform = thisTransform;
        _orientationService = orientationService;
        _stateChangeDelegate = stateChangeDelegate;
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

        _targetsFound = _targetFinder.FindTargets(_basicTargetFinderQuerySettings, _selfDamageReceiver, _orientationService.Forward);
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
