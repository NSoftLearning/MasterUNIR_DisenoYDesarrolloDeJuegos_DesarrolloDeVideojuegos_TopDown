using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAndCloseCombatEnemyController : MonoBehaviour
{
    ITargetFinder<IDamageReceiver, BasicTargetFinderQuerySettings> _targetFinder;
    
    [SerializeField] float _detectionRange;
    [SerializeField] List<SideSO> _sidesToSearchFor;
    [SerializeField] LayerMask layerstToSearchForTarget;
    [SerializeField] float _searchPersistenceTime;

    GenericStateMachine<PatrolAndCloseCombatStateId> _statesMachine;
    public void InjectDependencies (ITargetFinder<IDamageReceiver, BasicTargetFinderQuerySettings> targetFinder)
    {
        _targetFinder = targetFinder;
    }

    private void Start()
    {
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
                new BasicTargetFinderQuerySettings(
                    layerstToSearchForTarget,
                    _detectionRange,
                    _sidesToSearchFor,
                    transform),
                _targetFinder,
                GetComponent<IDamageReceiver>(),
                _statesMachine.FromStateToState),
            new SeekingState<PatrolAndCloseCombatStateId> (
                PatrolAndCloseCombatStateId.Seeking,
                PatrolAndCloseCombatStateId.Attacking,
                PatrolAndCloseCombatStateId.ReturningToPatrol,
                _searchPersistenceTime,
                new BasicTargetFinderQuerySettings(
                    layerstToSearchForTarget,
                    _detectionRange,
                    _sidesToSearchFor,
                    transform),
                _targetFinder,
                GetComponent<IDamageReceiver>(),
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
    }
}
 