using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PatrolAndCloseCombatEnemyController : MonoBehaviour
{
    [SerializeField] float _detectionRange;
    [SerializeField] float _closeDetectionRange;
    [SerializeField] List<DamageableTypeSO> _damageableTypesOfInterest;
    [SerializeField] LayerMask layerstToSearchForTarget;
    [SerializeField] float _searchPersistenceTime;
    [SerializeField] float _halfFieldOfView;
    [SerializeField] Transform _detectionOriginTransform;
    GenericStateMachine<PatrolAndCloseCombatStateId> _statesMachine;

    CustomCharacterController _characterController;
    ITargetFinder<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> _targetFinder;
    DetectionWithForwardAndIgnoreContext<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> _detecionStatesContext;

    IOrientationService _orientationService;
    DamageReceiverTargetSelector _targetSelector;
    
    public void InjectDependencies (
        ITargetFinder<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> targetFinder,
        IOrientationService orientationService,
        CustomCharacterController custmCharacterController)
    {
        _targetFinder = targetFinder;
        _orientationService = orientationService;
        _characterController = custmCharacterController;
    }

    private void Start()
    {
        _targetSelector = new DamageReceiverTargetSelector(_damageableTypesOfInterest);

        _detecionStatesContext =
            new DetectionWithForwardAndIgnoreContext<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>>(
                new DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>(
                    layerstToSearchForTarget,
                    _detectionRange,
                    _detectionOriginTransform,
                    _orientationService.Forward,
                    _halfFieldOfView,
                    _closeDetectionRange,
                    GetComponent<IDamageReceiver>()),
                _targetFinder,
                _orientationService,
                GetComponent<IDamageReceiver>());

        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        _statesMachine = new GenericStateMachine<PatrolAndCloseCombatStateId>();

        List<IGenericState<PatrolAndCloseCombatStateId>> states = new List<IGenericState<PatrolAndCloseCombatStateId>>
        {
            new InitialState <PatrolAndCloseCombatStateId> (
                PatrolAndCloseCombatStateId.InitialState,
                PatrolAndCloseCombatStateId.Patrolling,
                _statesMachine.FromStateToState),

            new PatrollingState <PatrolAndCloseCombatStateId> (
                PatrolAndCloseCombatStateId.Patrolling,
                PatrolAndCloseCombatStateId.Seeking,
                _detecionStatesContext,
                _targetSelector,
                _damageableTypesOfInterest,
                _statesMachine.FromStateToState),
            new SeekingState<PatrolAndCloseCombatStateId> (
                PatrolAndCloseCombatStateId.Seeking,
                PatrolAndCloseCombatStateId.Attacking,
                PatrolAndCloseCombatStateId.Patrolling,
                _searchPersistenceTime,
                _characterController,
                transform,
                _detecionStatesContext,
                _damageableTypesOfInterest,
                _targetSelector,
                _statesMachine.FromStateToState)                
        };

        _statesMachine.InitializeMachine(states, PatrolAndCloseCombatStateId.InitialState);
        _statesMachine.ChangeStateTo(PatrolAndCloseCombatStateId.Patrolling);
    }

    private void Update()
    {
        _statesMachine.Update();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_detectionOriginTransform.transform.position, _detectionRange);
        Gizmos.DrawWireSphere(_detectionOriginTransform.position, _closeDetectionRange);

        if (_orientationService == null)
            return;

        Vector3 origin = _orientationService.Position;
        Vector3 forward = _orientationService.Forward.normalized;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(_detectionOriginTransform.position, _detectionOriginTransform.position + forward * _detectionRange);

        Vector3 leftLimitDirection =
            Quaternion.AngleAxis(-_halfFieldOfView, Vector3.forward) * forward;

        Vector3 rightLimitDirection =
            Quaternion.AngleAxis(_halfFieldOfView, Vector3.forward) * forward;

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(_detectionOriginTransform.position, _detectionOriginTransform.position + leftLimitDirection.normalized * _detectionRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(_detectionOriginTransform.position, _detectionOriginTransform.position + rightLimitDirection.normalized * _detectionRange);
    }
}
 