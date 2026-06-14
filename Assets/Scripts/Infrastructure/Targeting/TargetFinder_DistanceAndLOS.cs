using System.Collections.Generic;

using UnityEngine;

public class TargetFinder_DistanceAndLOS<TARGET_TYPE> : ITargetFinder<TARGET_TYPE, BasicTargetFinderQuerySettings>
{ 

    public List<FoundTargetDTO<TARGET_TYPE>> FindTargets(BasicTargetFinderQuerySettings queryData, List<TARGET_TYPE> ignoreList)
    {
        Collider2D[] candidatesToTarget = Physics2D.OverlapCircleAll(
            queryData.origintransform.position, 
            queryData.range, 
            queryData.layersToSearch);

        List<FoundTargetDTO<TARGET_TYPE>> typedTargets = new();

        foreach (var item in candidatesToTarget)
        {
            TARGET_TYPE typedItem = item.GetComponent<TARGET_TYPE>();
            if (typedItem != null
                && !ignoreList.Contains(typedItem))
            {
                typedTargets.Add (new FoundTargetDTO<TARGET_TYPE>(typedItem));
            }
        }
        return typedTargets;
    }
}
