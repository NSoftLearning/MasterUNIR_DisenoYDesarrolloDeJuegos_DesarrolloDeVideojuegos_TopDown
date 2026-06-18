using UnityEngine;

public class ComponentsLibrary : MonoBehaviour
{
    public InventorySO InventorySO => _inventory;
    public SfxManager SfxManager => _sfxManager;
    public ITargetFinder<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> TargetFinderService_DistanceAndLOS => _damageableFinderService_DistanceAndLOS;
    public ITargetFinder<IDamageReceiver, BoxTargetFinderQuerySettings<IDamageReceiver>> TargetFinderService_BoxOverlap => _damageableFinderService_BoxOverlap;

    [SerializeField] InventorySO _inventory;
    [SerializeField] SfxManager _sfxManager;


    private ITargetFinder<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> _damageableFinderService_DistanceAndLOS;
    private ITargetFinder<IDamageReceiver, BoxTargetFinderQuerySettings<IDamageReceiver>> _damageableFinderService_BoxOverlap;

    private void Awake()
    {        
        _damageableFinderService_DistanceAndLOS = new TargetFinder_DistanceAndLOS<IDamageReceiver>();
        _damageableFinderService_BoxOverlap = new TargetFinder_BoxOverlap<IDamageReceiver>();

        ComponentLocatorService.BuildComponentsLibrary(this);

    }
}
