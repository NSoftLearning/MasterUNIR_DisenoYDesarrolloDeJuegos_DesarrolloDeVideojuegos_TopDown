using System;
using UnityEngine;

public class KamikazeRunningState<TStateId> : IGenericState<TStateId> where TStateId : Enum
{
    public TStateId StateId { get; }

    private CustomCharacterController _characterController;
    private float runningSpeed;
    private DetectionWithForwardAndIgnoreContext<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> context;
    private DamageOnCollision _collisionSensor;

    public KamikazeRunningState (
        TStateId stateId,
        CustomCharacterController characterController,
        float speed,
        DamageOnCollision collisionSensor,
        DetectionWithForwardAndIgnoreContext<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> context)
    {
        StateId = stateId;
        _characterController = characterController;
        runningSpeed = speed;
        this.context = context;
        _collisionSensor = collisionSensor;
    }
    public void Enter()
    {
        _characterController.MovementSpeed = runningSpeed;
        _characterController.SetRawMovement((ComponentLocatorService.Components.PlayerTransform.position - context.orientationService.Position).normalized);
        _collisionSensor.gameObject.SetActive(true);
    }

    public void Exit()
    {
        
    }

    public void Tick()
    {
        
    }
}
