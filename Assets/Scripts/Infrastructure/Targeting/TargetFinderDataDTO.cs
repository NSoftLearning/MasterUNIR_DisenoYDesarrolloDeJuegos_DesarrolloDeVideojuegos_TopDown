using System.Collections.Generic;
using UnityEngine;

public struct TargetFinderDataDTO
{
    public LayerMask layersToSearch;
    public float range;
    public List<SideSO> sidesToSearch;

}
