using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PatrolAndCloseCombatEnemyController : MonoBehaviour
{
    [SerializeField] float _detectionRange;
    [SerializeField] float _closeDetectionRange;
    [SerializeField] List<SideSO> _sidesToSearchFor;
    [SerializeField] LayerMask layerstToSearchForTarget;
    [SerializeField] float _searchPersistenceTime;
    [SerializeField] float _halfFieldOfView;

    GenericStateMachine<PatrolAndCloseCombatStateId> _statesMachine;

    CustomCharacterController _characterController;
    ITargetFinder<IDamageReceiver, BasicTargetFinderQuerySettings> _targetFinder;
    IOrientationService _orientationService;

    DetectionStatesContext _detecionStatesContext;
    public void InjectDependencies (
        ITargetFinder<IDamageReceiver, BasicTargetFinderQuerySettings> targetFinder,
        IOrientationService orientationService,
        CustomCharacterController custmCharacterController)
    {
        _targetFinder = targetFinder;
        _orientationService = orientationService;
        _characterController = custmCharacterController;
    }

    private void Start()
    {
        _detecionStatesContext = new DetectionStatesContext(
            new BasicTargetFinderQuerySettings(
                    layerstToSearchForTarget,
                    _detectionRange,
                    _sidesToSearchFor,
                    transform,
                    _halfFieldOfView,
                    _closeDetectionRange
                    ),
            _targetFinder,
            _orientationService,
            GetComponent<IDamageReceiver>()
            );

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
                _statesMachine.FromStateToState),
            new SeekingState<PatrolAndCloseCombatStateId> (
                PatrolAndCloseCombatStateId.Seeking,
                PatrolAndCloseCombatStateId.Attacking,
                PatrolAndCloseCombatStateId.Patrolling,
                _searchPersistenceTime,
                _characterController,
                transform,
                _detecionStatesContext,
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
        Gizmos.DrawWireSphere(transform.position, _detectionRange);

        if (_orientationService == null)
            return;

        Vector3 origin = _orientationService.Position;
        Vector3 forward = _orientationService.Forward.normalized;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(origin, origin + forward * _detectionRange);

        Vector3 leftLimitDirection =
            Quaternion.AngleAxis(-_halfFieldOfView, Vector3.forward) * forward;

        Vector3 rightLimitDirection =
            Quaternion.AngleAxis(_halfFieldOfView, Vector3.forward) * forward;

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(origin, origin + leftLimitDirection.normalized * _detectionRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(origin, origin + rightLimitDirection.normalized * _detectionRange);
    }
}
 