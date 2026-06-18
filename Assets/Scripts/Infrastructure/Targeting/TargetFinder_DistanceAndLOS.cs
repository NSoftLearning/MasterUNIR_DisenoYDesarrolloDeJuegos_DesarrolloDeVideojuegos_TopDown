using System.Collections.Generic;

using UnityEngine;

public class TargetFinder_DistanceAndLOS<TARGET_TYPE> : ITargetFinder<TARGET_TYPE, DistanceAndLosTargetFinderQuerySettings <TARGET_TYPE>>
{ 

    public List<FoundTargetDTO<TARGET_TYPE>> FindTargets(DistanceAndLosTargetFinderQuerySettings<TARGET_TYPE> queryData)
    {
        List<FoundTargetDTO<TARGET_TYPE>> typedTargetsCandidateList =  GetTypedTargetsByDistance(queryData);
        List<FoundTargetDTO<TARGET_TYPE>> typedTargetsFinalList = new List<FoundTargetDTO<TARGET_TYPE>>();

        foreach (FoundTargetDTO<TARGET_TYPE> item in typedTargetsCandidateList)
        {
            Vector3 fromOriginToTarget = item.position - queryData.origintransform.position;
            fromOriginToTarget.z = 0;
            queryData.originForward.z = 0;

            float signedAngle = Vector3.SignedAngle(queryData.originForward, fromOriginToTarget, Vector3.forward);
            if (Mathf.Abs(signedAngle) <= queryData.halfFieldOfView)
                typedTargetsFinalList.Add(item);         
        }        
        return typedTargetsFinalList;
    }

    List<FoundTargetDTO<TARGET_TYPE>> GetTypedTargetsByDistance(DistanceAndLosTargetFinderQuerySettings<TARGET_TYPE> queryData)
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
                && queryData.ignoredTarget != null
                && !queryData.ignoredTarget.Equals(typedItem))                   
            {
                typedTargets.Add(new FoundTargetDTO<TARGET_TYPE>(typedItem, item.transform.position));
            }
        }
        return typedTargets;
    }
}
