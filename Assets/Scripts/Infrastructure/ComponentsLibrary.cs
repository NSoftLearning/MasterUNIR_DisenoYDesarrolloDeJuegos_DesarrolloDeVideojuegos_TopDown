using UnityEngine;

public class ComponentsLibrary : MonoBehaviour
{
    public InventorySO InventorySO => _inventory;
    public ITargetFinder<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> TargetFinderService_DistanceAndLOS => _damageableFinderService_DistanceAndLOS;


    [SerializeField] InventorySO _inventory;


    private ITargetFinder<IDamageReceiver, DistanceAndLosTargetFinderQuerySettings<IDamageReceiver>> _damageableFinderService_DistanceAndLOS;
    private void Awake()
    {        
        _damageableFinderService_DistanceAndLOS = new TargetFinder_DistanceAndLOS<IDamageReceiver>();

        ComponentLocatorService.BuildComponentsLibrary(this);

    }
}
