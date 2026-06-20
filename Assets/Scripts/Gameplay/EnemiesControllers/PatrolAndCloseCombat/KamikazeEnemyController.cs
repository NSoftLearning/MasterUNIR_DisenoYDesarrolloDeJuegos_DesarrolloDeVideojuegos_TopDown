using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemyController : MonoBehaviour
{
    [SerializeField] LayerMask _lineOfSightBlockers;


    private GenericStateMachine<KamikazeStateId> _statesMachine;
    private DetectionWithForwardAndIgnoreContext<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> _detecionStatesContext;

    private void Start()
    {
        _statesMachine = new GenericStateMachine<KamikazeStateId>();
       /* _detecionStatesContext =
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
       */
        InitializeStatemachine();
    }

    private void InitializeStatemachine()
    {
        List<IGenericState<KamikazeStateId>> states = new List<IGenericState<KamikazeStateId>>
        {
            new InitialState <KamikazeStateId> (
                KamikazeStateId.InitialState,
                KamikazeStateId.Patrolling,
                _statesMachine.FromStateToState) };
    }
}
