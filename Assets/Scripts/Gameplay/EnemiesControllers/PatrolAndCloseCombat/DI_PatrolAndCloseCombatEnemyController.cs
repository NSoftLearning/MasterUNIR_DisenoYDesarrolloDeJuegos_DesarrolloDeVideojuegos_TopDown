using UnityEngine;

public class DI_PatrolAndCloseCombatEnemyController : MonoBehaviour
{
    [Header ("[[INJECTABLES]]")]
    [SerializeField] GameObject _characterRoot;
    [SerializeField] GameObject _enemyAttackObject;

    [Header("[[ INJECTION TARGETS]]")]
    [SerializeField] PatrolAndCloseCombatEnemyController patrolAndCloseCombatController;


    private void Awake()
    {
        _enemyAttackObject.GetComponent<CloseQuartersAttack>().InjectDependencies(
            ComponentLocatorService.Components.TargetFinderService_BoxOverlap,
            _characterRoot.GetComponent<IDamageReceiver>(),
            _characterRoot.GetComponent<IOrientationService>());

        patrolAndCloseCombatController
            .InjectDependencies(
            ComponentLocatorService.Components.TargetFinderService_DistanceAndLOS,
            _characterRoot.GetComponent<IOrientationService>(),
            _characterRoot.GetComponent<CustomCharacterController>(),
            _enemyAttackObject.GetComponent<IEnemyAttack>());
    }
}

