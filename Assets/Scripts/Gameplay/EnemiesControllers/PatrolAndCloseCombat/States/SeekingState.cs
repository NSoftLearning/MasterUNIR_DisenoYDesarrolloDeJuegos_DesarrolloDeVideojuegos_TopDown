using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SeekingState<TStateId> : IGenericState<TStateId> where TStateId : Enum
{
    public TStateId StateId { get; }

    private TStateId _handleTargetReached;
    private TStateId _handleTargetLost;
    private BasicTargetFinderQuerySettings _basicTargetFinderQuerySettings;
    private ITargetFinder<IDamageReceiver, BasicTargetFinderQuerySettings> _targetFinder;
    private List<IDamageReceiver> _selfDamageReceiver;
    private StateChangeDelegate<TStateId> _stateChangeDelegate;
    float _searchPersistenceTime;

    float _willDesistAt = 0;
    public SeekingState (
        TStateId thisStateId,
        TStateId handleTargetReached,
        TStateId handleTargetMissed,
        float searchPersistenceTime,
        BasicTargetFinderQuerySettings basicTargetFinderQuerySettings,
        ITargetFinder <IDamageReceiver, BasicTargetFinderQuerySettings> targetFinder,
        IDamageReceiver selfDamageReceiver,
        StateChangeDelegate <TStateId> stateChangeDelegate)
    {
        StateId = thisStateId;
        _handleTargetReached = handleTargetReached;
        _handleTargetLost = handleTargetMissed;
        _searchPersistenceTime = searchPersistenceTime;
        _basicTargetFinderQuerySettings = basicTargetFinderQuerySettings;
        _targetFinder = targetFinder;
        _selfDamageReceiver = new List<IDamageReceiver> { selfDamageReceiver };
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


    }

    private bool ChangeStateDueTargetLost()
    {
        List<FoundTargetDTO<IDamageReceiver>> targetsFound = _targetFinder.FindTargets(_basicTargetFinderQuerySettings, _selfDamageReceiver);
        if (targetsFound.Count > 0)
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
