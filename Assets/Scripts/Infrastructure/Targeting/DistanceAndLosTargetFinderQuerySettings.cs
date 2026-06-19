using System.Collections.Generic;
using UnityEngine;

public struct DistanceAndLosTargetFinderQuerySettings <TARGET_TYPE> : IOrientedTargetFinderQuery, ITargetFinderWithIgnoreQuery<TARGET_TYPE>
{
    public LayerMask layersToSearch;
    public LayerMask lineOfSightBlockers;
    public float range;
    public Transform origintransform;
    public Vector3 originForward;
    public float halfFieldOfView;
    public float closeRange;
    public TARGET_TYPE ignoredTarget;

    public Vector3 OriginForward { 
        get => originForward;
        set => originForward = value; }
    public TARGET_TYPE IgnoredTarget { get => ignoredTarget; set =>ignoredTarget = value; }

    public DistanceAndLosTargetFinderQuerySettings (
        LayerMask lineOfSicghtBlockers,
        LayerMask layersToSearch,
        float detectionRange,  

        Transform originTransform,
        Vector3 originForward,
        float halfFieldOfView,
        float closeRange,
        TARGET_TYPE ignoredTarget)
    {
        this.lineOfSightBlockers = lineOfSicghtBlockers;
        this.layersToSearch = layersToSearch;
        this.range = detectionRange;
        this.origintransform = originTransform;
        this.closeRange = closeRange;
        this.halfFieldOfView = halfFieldOfView;
        this.originForward = originForward;
        this.ignoredTarget = ignoredTarget;


    }

    
}
