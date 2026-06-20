using UnityEngine;
using UnityEngine.AI;

public class NavMeshDirectionFindingService : MonoBehaviour, IDirectionFindingService
{
    [SerializeField]
    private float _sampleDistance = 0.5f;

    [SerializeField]
    private float _navMeshZ = 0f;

    private NavMeshPath _path ;

    void Awake()
    {
        _path = new();
    }
    public bool TryGetDirection(Vector3 origin, Vector3 destination, out Vector3 directionToNextCorner, out Vector3 nextCornerPosition)
    {
        directionToNextCorner = Vector3.zero;
        nextCornerPosition = Vector3.zero;

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
        Debug.Log($"Raw navmesh corner: {_path.corners[1]}");

        nextCornerPosition = new Vector3(nextWaypoint.x, nextWaypoint.y, 0f);

        Vector3 originOnMovementPlane = new Vector3(origin.x, origin.y, 0f);

        Vector3 fromCurrentPositionToNextWaypoint = nextCornerPosition - originOnMovementPlane;
        fromCurrentPositionToNextWaypoint.z = 0f;

        if (fromCurrentPositionToNextWaypoint.sqrMagnitude <= 0.0001f)
            return false;

        directionToNextCorner = fromCurrentPositionToNextWaypoint.normalized;
        return true;
    }
}
