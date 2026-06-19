using System.Collections.Generic;
using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    [SerializeField] List<PatrolWaypoint> _waypoints;



    public PatrolWaypoint GetClosestWaypoint (Vector3 position) 
    {
            PatrolWaypoint currentClosest = null;
            float distanceToCurrentClosest = Mathf.Infinity;
            foreach (PatrolWaypoint waypoint in _waypoints)
            {
                float distanceToThis = Vector3.Distance(position, waypoint.transform.position);

                if (distanceToThis < distanceToCurrentClosest)
                {
                    currentClosest = waypoint;
                    distanceToCurrentClosest = distanceToThis;
                 }
            }

            return currentClosest;
    } 

    public PatrolWaypoint GetNextWaypoint (PatrolWaypoint reachedWaypoint, ref bool goingForward)
    {
        int indexOfReachedWaypoint = _waypoints.IndexOf(reachedWaypoint);
        if (indexOfReachedWaypoint == 0)
            goingForward = true;
        if (indexOfReachedWaypoint == _waypoints.Count - 1)
            goingForward = false;

        if (goingForward)
            return _waypoints[indexOfReachedWaypoint + 1];
        else
            return _waypoints[indexOfReachedWaypoint - 1];

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blueViolet;
        for (int i = 0; i < _waypoints.Count - 1 ; i++)
        {
            Gizmos.DrawLine(
                _waypoints[i].transform.position,
                _waypoints[i+1].transform.position);
        }

    }
}
