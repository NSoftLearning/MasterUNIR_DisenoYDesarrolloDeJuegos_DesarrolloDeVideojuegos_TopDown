using UnityEngine;

public class DI_KamikazeEnemyController : MonoBehaviour
{
    [Header("[[INJECTABLES]]")]
    [SerializeField] GameObject _characterRoot;

    [Header("[[ INJECTION TARGETS]]")]
    [SerializeField] KamikazeEnemyController _kamikazeEnemyController;


    private void Awake()
    {
        _kamikazeEnemyController
            .InjectDependencies(
            ComponentLocatorService.Components.TargetFinderService_DistanceAndLOS,
            _characterRoot.GetComponent<IOrientationService>(),
            _characterRoot.GetComponent<CustomCharacterController>(),
            ComponentLocatorService.Components.DirectionFindingService);
    }
}
