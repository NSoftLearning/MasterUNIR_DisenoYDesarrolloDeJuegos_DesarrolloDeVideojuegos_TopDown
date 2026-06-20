using UnityEngine;

public class ComponentsLibrary : MonoBehaviour
{
    public Transform PlayerTransform => playerTransform;
    public InventoryManager InventoryManager => _inventory;
    public SfxManager SfxManager => _sfxManager;
    public ITargetFinder<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> TargetFinderService_DistanceAndLOS => _damageableFinderService_DistanceAndLOS;
    public ITargetFinder<IDamageReceiver, BoxTargetFinderQuerySettings<IDamageReceiver>> TargetFinderService_BoxOverlap => _damageableFinderService_BoxOverlap;
    public IDirectionFindingService DirectionFindingService { get; set; }

    [SerializeField] InventoryManager _inventory;
    [SerializeField] SfxManager _sfxManager;
    [SerializeField] GameObject _directionFindingService;

    private ITargetFinder<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> _damageableFinderService_DistanceAndLOS;
    private ITargetFinder<IDamageReceiver, BoxTargetFinderQuerySettings<IDamageReceiver>> _damageableFinderService_BoxOverlap;
    private Transform playerTransform;
    private void Awake()
    {        
        _damageableFinderService_DistanceAndLOS = new TargetFinder_DistanceAndLOS<IDamageReceiver>();
        _damageableFinderService_BoxOverlap = new TargetFinder_BoxOverlap<IDamageReceiver>();

        DirectionFindingService = _directionFindingService.GetComponent<IDirectionFindingService>();
        ComponentLocatorService.BuildComponentsLibrary(this);

    }

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }
}
