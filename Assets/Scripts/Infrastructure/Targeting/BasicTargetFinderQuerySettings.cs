using System.Collections.Generic;
using UnityEngine;

public struct BasicTargetFinderQuerySettings
{
    public LayerMask layersToSearch;
    public float range;
    public List<SideSO> sidesToSearch;
    public Transform origintransform;
    

    public BasicTargetFinderQuerySettings (
        LayerMask layersToSearch,
        float range, List<SideSO> sidesToSearch, 
        Transform originTransform)
    {
        this.layersToSearch = layersToSearch;
        this.range = range;
        this.sidesToSearch = sidesToSearch;
        this.origintransform = originTransform;
    }
}
