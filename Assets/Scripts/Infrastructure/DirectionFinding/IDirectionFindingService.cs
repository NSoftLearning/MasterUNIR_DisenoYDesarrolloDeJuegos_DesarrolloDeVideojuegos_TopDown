using UnityEngine;

public interface IDirectionFindingService
{
    bool TryGetDirection(Vector3 origin, Vector3 destination, out Vector3 direction, out Vector3 position);
    
}
