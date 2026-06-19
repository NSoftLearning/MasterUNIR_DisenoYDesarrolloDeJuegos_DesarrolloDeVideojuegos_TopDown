using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PatrolAndCloseCombatEnemyController : MonoBehaviour
{
    [SerializeField] float _unawareDetectionRange;
    [SerializeField] float _alertDetectionRange;
    [SerializeField] float closeQuartersRange;
    [SerializeField] List<DamageableTypeSO> _damageableTypesOfInterest;
    [SerializeField] LayerMask layerstToSearchForTarget;
    [SerializeField] private LayerMask _lineOfSightBlockers;
    [SerializeField] float _searchPersistenceTime;
    [SerializeField] float _halfFieldOfView;
    [SerializeField] Transform _detectionOriginTransform;
    [SerializeField] PatrolRoute _patrolRoute;
    
    GenericStateMachine<PatrolAndCloseCombatStateId> _statesMachine;

    CustomCharacterController _characterController;
    ITargetFinder<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> _targetFinder;
    DetectionWithForwardAndIgnoreContext<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> _detecionStatesContext;
    IEnemyAttack _enemyAttack;
    IOrientationService _orientationService;
    IDirectionFindingService _directionFindingService;
    DamageReceiverTargetSelector _targetSelector;
    

    public void InjectDependencies (
        ITargetFinder<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> targetFinder,
        IOrientationService orientationService,
        CustomCharacterController custmCharacterController,
        IEnemyAttack enemyAttack,
        IDirectionFindingService directionFindingService)
    {
        _targetFinder = targetFinder;
        _orientationService = orientationService;
        _characterController = custmCharacterController;
        _enemyAttack = enemyAttack;
        _directionFindingService = directionFindingService;
    }

    private void Start()
    {
        _targetSelector = new DamageReceiverTargetSelector(_damageableTypesOfInterest);

        _detecionStatesContext =
            new DetectionWithForwardAndIgnoreContext<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>>(
                new DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>(
                    _lineOfSightBlockers,
                    layerstToSearchForTarget,
                    _unawareDetectionRange,
                    _detectionOriginTransform,
                    _orientationService.Forward,
                    _halfFieldOfView,
                    closeQuartersRange,
                    GetComponent<IDamageReceiver>()
                    ),
                _targetFinder,
                _orientationService,
                _patrolRoute,
                _directionFindingService,
                GetComponent<IDamageReceiver>(),
                _characterController);

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
                transform,
                _detecionStatesContext,
                _targetSelector,
                _enemyAttack,
                _damageableTypesOfInterest,
                _alertDetectionRange,
                _statesMachine.FromStateToState),
            new AttackingStatee <PatrolAndCloseCombatStateId> (
                PatrolAndCloseCombatStateId.Attacking,
                PatrolAndCloseCombatStateId.Patrolling,
                _enemyAttack,
                layerstToSearchForTarget, 
                _damageableTypesOfInterest,
                _detectionOriginTransform,
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
        Gizmos.DrawWireSphere(_detectionOriginTransform.transform.position, _unawareDetectionRange);
        Gizmos.DrawWireSphere(_detectionOriginTransform.position, closeQuartersRange);
        Gizmos.DrawWireSphere(_detectionOriginTransform.transform.position, _alertDetectionRange);

        if (_orientationService == null)
            return;

        Vector3 origin = _orientationService.Position;
        Vector3 forward = _orientationService.Forward.normalized;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(_detectionOriginTransform.position, _detectionOriginTransform.position + forward * _unawareDetectionRange);

        Vector3 leftLimitDirection =
            Quaternion.AngleAxis(-_halfFieldOfView, Vector3.forward) * forward;

        Vector3 rightLimitDirection =
            Quaternion.AngleAxis(_halfFieldOfView, Vector3.forward) * forward;

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(_detectionOriginTransform.position, _detectionOriginTransform.position + leftLimitDirection.normalized * _unawareDetectionRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(_detectionOriginTransform.position, _detectionOriginTransform.position + rightLimitDirection.normalized * _unawareDetectionRange);
    }
}
 