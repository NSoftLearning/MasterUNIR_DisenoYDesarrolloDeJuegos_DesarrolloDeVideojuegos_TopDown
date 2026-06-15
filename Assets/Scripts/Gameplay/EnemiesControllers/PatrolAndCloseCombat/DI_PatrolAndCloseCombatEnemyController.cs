using UnityEngine;

public class DI_PatrolAndCloseCombatEnemyController : MonoBehaviour
{
    [Header ("[[INJECTABLES]]")]
    [SerializeField] GameObject _characterRoot;

    [Header("[[ INJECTION TARGETS]]")]
    [SerializeField] PatrolAndCloseCombatEnemyController patrolAndCloseCombatController;


    private void Awake()
    {      
        patrolAndCloseCombatController
            .InjectDependencies(
            ComponentLocatorService.Components.TargetFinderService_DistanceAndLOS,
            _characterRoot.GetComponent<IOrientationService>(),
            _characterRoot.GetComponent<CustomCharacterController>());
    }
}

