using UnityEngine;

public class ComponentsLibrary : MonoBehaviour
{
    public InventorySO InventorySO => _inventory;
    public ITargetFinder<IDamageReceiver> TargetFinderService_DistanceAndLOS => _targetFinderService_DistanceAndLOS;


    [SerializeField] InventorySO _inventory;


    public ITargetFinder<IDamageReceiver> _targetFinderService_DistanceAndLOS;
    private void Awake()
    {
        ComponentLocatorService.BuildComponentsLibrary(this);
        _targetFinderService_DistanceAndLOS = new DamageableTargetFinder_DistanceAndLOS();

    }
}
