using UnityEngine;

public class ComponentsLibrary : MonoBehaviour
{
    public InventorySO InventorySO => _inventory;
    public ITargetFinder<IDamageReceiver, BasicTargetFinderQuerySettings<IDamageReceiver>> TargetFinderService_DistanceAndLOS => _damageableFinderService_DistanceAndLOS;


    [SerializeField] InventorySO _inventory;


    private ITargetFinder<IDamageReceiver, BasicTargetFinderQuerySettings<IDamageReceiver>> _damageableFinderService_DistanceAndLOS;
    private void Awake()
    {        
        _damageableFinderService_DistanceAndLOS = new TargetFinder_DistanceAndLOS<IDamageReceiver>();

        ComponentLocatorService.BuildComponentsLibrary(this);

    }
}
