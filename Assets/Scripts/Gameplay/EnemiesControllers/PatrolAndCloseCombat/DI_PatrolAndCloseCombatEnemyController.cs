using UnityEngine;

public class DI_PatrolAndCloseCombatEnemyController : MonoBehaviour
{
    [Header ("[[INJECTABLES]]")]
    [SerializeField] GameObject _characterRoot;
    [SerializeField] GameObject _enemyAttackObject;
    [SerializeField] GameObject _damageReceiver;

    [Header("[[ INJECTION TARGETS]]")]
    [SerializeField] PatrolAndCloseCombatEnemyController patrolAndCloseCombatController;


    [Header("[[CALLBACKS]]")]
    [SerializeField] EnemyFeedback enemyFeedback;

    private void Awake()
    {
        _enemyAttackObject.GetComponent<CloseQuartersAttack>().InjectDependencies(
            ComponentLocatorService.Components.TargetFinderService_BoxOverlap,
            _characterRoot.GetComponent<IDamageReceiver>(),
            _characterRoot.GetComponent<IOrientationService>(),
            _characterRoot.GetComponentInChildren<AnimationEventsAdapter>());

        patrolAndCloseCombatController
            .InjectDependencies(
            ComponentLocatorService.Components.TargetFinderService_DistanceAndLOS,
            _characterRoot.GetComponent<IOrientationService>(),
            _characterRoot.GetComponent<CustomCharacterController>(),
            _enemyAttackObject.GetComponent<IEnemyAttack>(),
            ComponentLocatorService.Components.DirectionFindingService);     
    }

    private void OnEnable()
    {
        _damageReceiver.GetComponent<IDamageReceiver>().Died += enemyFeedback.ShowFeedbackDied;
    }

    private void OnDisable()
    {
        _damageReceiver.GetComponent<IDamageReceiver>().Died += enemyFeedback.ShowFeedbackDied;
    }
}

