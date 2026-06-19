using UnityEngine;

public interface IDirectionFindingService
{
    bool TryGetDirection(Vector2 origin, Vector2 destination, out Vector2 direction);
    
}
