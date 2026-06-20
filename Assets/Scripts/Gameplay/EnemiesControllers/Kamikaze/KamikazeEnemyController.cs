using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemyController : MonoBehaviour
{
    [SerializeField] LayerMask _lineOfSightBlockers;
    [SerializeField] LayerMask layerstToSearchForTarget;
    [SerializeField] float _unawareDetectionRange;
    [SerializeField] Transform _detectionOriginTransform;
    [SerializeField] float _halfFieldOfView;
    [SerializeField] float closeQuartersRange;
    [SerializeField] PatrolRoute _patrolRoute;
    [SerializeField] List<DamageableTypeSO> _damageableTypesOfInterest;
    [SerializeField] float _runningDelay;
    [SerializeField] float _runningSpeed;
    [SerializeField] DamageOnCollision _collisionDetector;
    private GenericStateMachine<KamikazeStateId> _statesMachine;
    private DetectionWithForwardAndIgnoreContext<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> _detecionStatesContext;

    CustomCharacterController _characterController;
    IDirectionFindingService _directionFindingService;
    ITargetFinder<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> _targetFinder;
    IOrientationService _orientationService;
    DamageReceiverTargetSelector _targetSelector;
    public void InjectDependencies(
    ITargetFinder<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> targetFinder,
    IOrientationService orientationService,
    CustomCharacterController custmCharacterController,
    IDirectionFindingService directionFindingService)
    {
        _targetFinder = targetFinder;
        _orientationService = orientationService;
        _characterController = custmCharacterController;
        _directionFindingService = directionFindingService;
    }
    private void Start()
    {
        _collisionDetector.enabled = false;
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
       
        InitializeStatemachine();

        _collisionDetector.Initialize(GetComponent<Collider2D>());
    }

    private void InitializeStatemachine()
    {
        _statesMachine = new GenericStateMachine<KamikazeStateId>();
        List<IGenericState<KamikazeStateId>> states = new List<IGenericState<KamikazeStateId>>
        {
            new InitialState <KamikazeStateId> (
                KamikazeStateId.InitialState,
                KamikazeStateId.GettingReadyToRun,
                _statesMachine.FromStateToState),
        new PatrollingState <KamikazeStateId> (
                KamikazeStateId.Patrolling,
                KamikazeStateId.GettingReadyToRun,  
                _detecionStatesContext,
                _targetSelector,
                _damageableTypesOfInterest,
                _statesMachine.FromStateToState),
        new GettingReadyToRunState<KamikazeStateId> (
            KamikazeStateId.GettingReadyToRun, 
            KamikazeStateId.Running,
            _runningDelay, 
            _characterController, 
            _statesMachine.FromStateToState),
        new KamikazeRunningState<KamikazeStateId> (
            KamikazeStateId.Running,
            _characterController,
            _runningSpeed, 
            _collisionDetector,
            _detecionStatesContext
            )};

        _statesMachine.InitializeMachine(states, KamikazeStateId.InitialState);
        _statesMachine.ChangeStateTo(KamikazeStateId.Patrolling);
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
        //Gizmos.DrawWireSphere(_detectionOriginTransform.transform.position, _alertDetectionRange);

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
