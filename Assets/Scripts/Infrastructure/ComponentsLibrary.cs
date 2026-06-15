using UnityEngine;

public class ComponentsLibrary : MonoBehaviour
{
    public InventorySO InventorySO => _inventory;
    public ITargetFinder<IDamageReceiver, BasicTargetFinderQuerySettings> TargetFinderService_DistanceAndLOS => _damageableFinderService_DistanceAndLOS;


    [SerializeField] InventorySO _inventory;


    public ITargetFinder<IDamageReceiver, BasicTargetFinderQuerySettings> _damageableFinderService_DistanceAndLOS;
    private void Awake()
    {
        ComponentLocatorService.BuildComponentsLibrary(this);
        _damageableFinderService_DistanceAndLOS = new TargetFinder_DistanceAndLOS<IDamageReceiver>();

    }
}
