using System.Collections.Generic;
using UnityEngine;

public struct BasicTargetFinderQuerySettings
{
    public LayerMask layersToSearch;
    public float range;
    public List<SideSO> sidesToSearch;
    public Transform origintransform;
   // public Vector3 originForward;
    public float halfFieldOfView;
    public float closeRange;
    

    public BasicTargetFinderQuerySettings (
        LayerMask layersToSearch,
        float range, 
        List<SideSO> sidesToSearch, 
        Transform originTransform,
     //   Vector3 originForward,
        float halfFieldOfView,
        float closeRange)
    {
        this.layersToSearch = layersToSearch;
        this.range = range;
        this.sidesToSearch = sidesToSearch;
        this.origintransform = originTransform;
        this.closeRange = closeRange;
        this.halfFieldOfView = halfFieldOfView;
    }
}
