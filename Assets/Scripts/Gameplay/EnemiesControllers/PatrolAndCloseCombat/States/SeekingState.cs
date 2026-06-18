
using System;
using System.Collections.Generic;
using UnityEngine;

public class SeekingState<TStateId> : IGenericState<TStateId> where TStateId : Enum
{
    public TStateId StateId { get; }

    private TStateId _handleTargetReached;
    private TStateId _handleTargetLost;
    private CustomCharacterController _customCharacterController;
    private Transform _transform;

    private StateChangeDelegate<TStateId> _stateChangeDelegate;
    private List<DamageableTypeSO> _damageableTypesOfInterest;
    private DamageReceiverTargetSelector _targetSelector;
    DetectionWithForwardAndIgnoreContext<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> _detectionContext;
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
        DetectionWithForwardAndIgnoreContext<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>>  detectionContext,
        List<DamageableTypeSO> damageableTypesOfInterest,
        DamageReceiverTargetSelector targetSelector,
        StateChangeDelegate <TStateId> stateChangeDelegate)
    {
        StateId = thisStateId;
        _handleTargetReached = handleTargetReached;
        _handleTargetLost = handleTargetMissed;
        _searchPersistenceTime = searchPersistenceTime;
        _customCharacterController = customCharacterController;
        _transform = thisTransform;
        _stateChangeDelegate = stateChangeDelegate;
        _damageableTypesOfInterest = damageableTypesOfInterest;
        _targetSelector = targetSelector; 
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
        List<FoundTargetDTO<IDamageReceiver>> detectedTargets =
            _detectionContext.targetFinder.FindTargets(_detectionContext.GetCurrentQueryData());

        if (_targetSelector.TryGetTargetOfInterest(
                detectedTargets,
                out FoundTargetDTO<IDamageReceiver> targetOfInterest))
        {
            _targetsFound.Clear();
            _targetsFound.Add(targetOfInterest);

            _willDesistAt = Time.time + _searchPersistenceTime;
            return false;
        }

        if (Time.time > _willDesistAt)
        {
            _stateChangeDelegate.Invoke(StateId, _handleTargetLost);
            return true;
        }

        return false;
    }
}
