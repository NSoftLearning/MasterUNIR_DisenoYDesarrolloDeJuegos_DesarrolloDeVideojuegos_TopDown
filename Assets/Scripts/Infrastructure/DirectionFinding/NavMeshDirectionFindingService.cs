using UnityEngine;
using UnityEngine.AI;

public class NavMeshDirectionFindingService : MonoBehaviour, IDirectionFindingService
{
    [SerializeField]
    private float _sampleDistance = 0.5f;

    [SerializeField]
    private float _navMeshZ = 0f;

    private NavMeshPath _path ;

    void Start()
    {
        _path = new();
    }
    public bool TryGetDirection(Vector2 origin, Vector2 destination, out Vector2 direction)
    {
        direction = Vector2.zero;

        Vector3 origin3D = new Vector3(origin.x, origin.y, _navMeshZ);
        Vector3 destination3D = new Vector3(destination.x, destination.y, _navMeshZ);

        if (!NavMesh.SamplePosition(origin3D, out NavMeshHit originInMesh, _sampleDistance, NavMesh.AllAreas))
            return false;

        if (!NavMesh.SamplePosition(destination3D, out NavMeshHit destinationInMesh, _sampleDistance, NavMesh.AllAreas))
            return false;

        if (!NavMesh.CalculatePath(originInMesh.position, destinationInMesh.position, NavMesh.AllAreas, _path))
            return false;

        if (_path.status != NavMeshPathStatus.PathComplete)
            return false;

        if (_path.corners.Length < 2)
            return false;

        Vector3 nextWaypoint = _path.corners[1];

        Vector2 fromCurrentPositionToNextWaypoint = new Vector2(
            nextWaypoint.x - origin.x,
            nextWaypoint.y - origin.y
        );

        if (fromCurrentPositionToNextWaypoint.sqrMagnitude <= 0.0001f)
            return false;

        direction = fromCurrentPositionToNextWaypoint.normalized;
        return true;
    }
}
