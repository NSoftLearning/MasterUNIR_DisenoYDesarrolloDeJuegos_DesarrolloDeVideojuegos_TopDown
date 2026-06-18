
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
    //private List<DamageableTypeSO> _damageableTypesOfInterest;
    private DamageReceiverTargetSelector _targetSelector;
    DetectionWithForwardAndIgnoreContext<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> _detectionContext;
    private IEnemyAttack _enemyAttack;
    float _searchPersistenceTime;


    List<FoundTargetDTO<IDamageReceiver>> _currentTargetsOfInterest = new ();
    float _willDesistAt = 0;
    public SeekingState(
        TStateId thisStateId,
        TStateId handleTargetReached,
        TStateId handleTargetMissed,
        float searchPersistenceTime,
        CustomCharacterController customCharacterController,
        Transform thisTransform,
        DetectionWithForwardAndIgnoreContext<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> detectionContext,
       // List<DamageableTypeSO> damageableTypesOfInterest,
        DamageReceiverTargetSelector targetSelector,
        IEnemyAttack enemyAttack,
        StateChangeDelegate <TStateId> stateChangeDelegate)
    {
        StateId = thisStateId;
        _handleTargetReached = handleTargetReached;
        _handleTargetLost = handleTargetMissed;
        _searchPersistenceTime = searchPersistenceTime;
        _customCharacterController = customCharacterController;
        _transform = thisTransform;
        _stateChangeDelegate = stateChangeDelegate;
       // _damageableTypesOfInterest = damageableTypesOfInterest;
        _targetSelector = targetSelector; 
        _detectionContext = detectionContext;
        _enemyAttack = enemyAttack;
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
        RefreshDetectedTargets();
      // if (_currentTargetsOfInterest.Count == 0)
        //{
          //  _willDesistAt = Time.time + _searchPersistenceTime;
       // }
       

        if (_currentTargetsOfInterest.Count > 0)
        {
            _customCharacterController.SetRawMovement((_currentTargetsOfInterest[0].target.GetPosition() - _transform.position).normalized);
            _willDesistAt = Time.time + _searchPersistenceTime;
        }
        
        if (Time.time > _willDesistAt)
        {
            _stateChangeDelegate.Invoke(StateId, _handleTargetLost);
            return;            
        }
    }

    private void RefreshDetectedTargets()
    {
        _currentTargetsOfInterest.Clear();
        List<FoundTargetDTO<IDamageReceiver>> detectedTargets =
            _detectionContext.targetFinder.FindTargets(_detectionContext.GetCurrentQueryData());

        if (_targetSelector.TryGetTargetOfInterest(
                detectedTargets,
                out FoundTargetDTO<IDamageReceiver> targetOfInterest))
        {
            
            _currentTargetsOfInterest.Add(targetOfInterest);

           // _willDesistAt = Time.time + _searchPersistenceTime;
           // return false;
        }

   //     if (Time.time > _willDesistAt)
     //   {
   //         _stateChangeDelegate.Invoke(StateId, _handleTargetLost);
  //          return true;
   //     }

   //     return false;
    }


}
