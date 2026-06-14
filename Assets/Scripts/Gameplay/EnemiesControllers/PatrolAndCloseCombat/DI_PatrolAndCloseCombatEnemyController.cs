using UnityEngine;

public class DI_PatrolAndCloseCombatEnemyController : MonoBehaviour
{
    [Header ("[[INJECTABLES]]")]
    [SerializeField] GameObject TargetFinder;

    [Header("[[ INJECTION TARGETS]]")]
    [SerializeField] PatrolAndCloseCombatEnemyController patrolAndCloseCombatController;


    private void Awake()
    {      
        patrolAndCloseCombatController
            .InjectDependencies(ComponentLocatorService.Components.TargetFinderService_DistanceAndLOS);
    }
}

