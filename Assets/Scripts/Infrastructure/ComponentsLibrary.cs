using UnityEngine;

public class ComponentsLibrary : MonoBehaviour
{
    public InventoryManager InventoryManager => _inventory;
    public SfxManager SfxManager => _sfxManager;
    public ITargetFinder<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> TargetFinderService_DistanceAndLOS => _damageableFinderService_DistanceAndLOS;


    [SerializeField] InventoryManager _inventory;
    [SerializeField] SfxManager _sfxManager;


    private ITargetFinder<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> _damageableFinderService_DistanceAndLOS;
    private void Awake()
    {        
        _damageableFinderService_DistanceAndLOS = new TargetFinder_DistanceAndLOS<IDamageReceiver>();

        ComponentLocatorService.BuildComponentsLibrary(this);

    }
}
