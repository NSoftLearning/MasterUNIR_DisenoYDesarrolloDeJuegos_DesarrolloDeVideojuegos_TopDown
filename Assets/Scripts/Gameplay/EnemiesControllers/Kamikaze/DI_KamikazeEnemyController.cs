using UnityEngine;

public class DI_KamikazeEnemyController : MonoBehaviour
{
    [Header("[[INJECTABLES]]")]
    [SerializeField] GameObject _characterRoot;
    [SerializeField] GameObject _damageReceiver;
    [SerializeField] DamageOnCollision _damageOnCollision;
    [Header("[[ INJECTION TARGETS]]")]
    [SerializeField] KamikazeEnemyController _kamikazeEnemyController;

    [Header("[[CALLBACKS]]")]
    [SerializeField] EnemyFeedback enemyFeedbackDied;
    [SerializeField] EnemyFeedback kamikazeHit;
    
    private void Awake()
    {
        _kamikazeEnemyController
            .InjectDependencies(
            ComponentLocatorService.Components.TargetFinderService_DistanceAndLOS,
            _characterRoot.GetComponent<IOrientationService>(),
            _characterRoot.GetComponent<CustomCharacterController>(),
            ComponentLocatorService.Components.DirectionFindingService);
    }


    private void OnEnable()
    {
        _damageReceiver.GetComponent<IDamageReceiver>().Died += enemyFeedbackDied.ShowFeedbackDied;
        _damageOnCollision.HitSomething += kamikazeHit.ShowFeedbackDied;
    }

    private void OnDisable()
    {
        _damageReceiver.GetComponent<IDamageReceiver>().Died -= enemyFeedbackDied.ShowFeedbackDied;
        _damageOnCollision.HitSomething -= kamikazeHit.ShowFeedbackDied;
    }
}
