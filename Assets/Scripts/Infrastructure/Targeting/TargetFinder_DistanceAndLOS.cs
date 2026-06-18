using System.Collections.Generic;
using UnityEngine;

public class TargetFinder_DistanceAndLOS<TARGET_TYPE> : ITargetFinder<TARGET_TYPE, DistanceAndLosTargetFinderQuerySettings <TARGET_TYPE>>
{ 

    public List<FoundTargetDTO<TARGET_TYPE>> FindTargets(DistanceAndLosTargetFinderQuerySettings<TARGET_TYPE> queryData)
    {               
        List<FoundTargetDTO<TARGET_TYPE>> typedTargetsCandidateList =  GetTypedTargetsByDistance(queryData, queryData.range);
        HashSet<FoundTargetDTO<TARGET_TYPE>> typedTargetsFinal_HS = new HashSet<FoundTargetDTO<TARGET_TYPE>>();

        Vector3 originForward = queryData.originForward;
        originForward.z = 0f;
        foreach (FoundTargetDTO<TARGET_TYPE> item in typedTargetsCandidateList)
        {
            Vector3 fromOriginToTarget = item.position - queryData.origintransform.position;
            fromOriginToTarget.z = 0;

            float signedAngle = Vector3.SignedAngle(originForward, fromOriginToTarget, Vector3.forward);
            if (Mathf.Abs(signedAngle) <= queryData.halfFieldOfView)
                typedTargetsFinal_HS.Add(item);         
        }

        typedTargetsFinal_HS.UnionWith(GetTypedTargetsByDistance(queryData, queryData.closeRange));


         return new List<FoundTargetDTO<TARGET_TYPE>>(typedTargetsFinal_HS);
    }

    List<FoundTargetDTO<TARGET_TYPE>> GetTypedTargetsByDistance(DistanceAndLosTargetFinderQuerySettings<TARGET_TYPE> queryData, float range)
    {
        Collider2D[] candidatesToTarget = Physics2D.OverlapCircleAll(
            queryData.origintransform.position,
            range,
            queryData.layersToSearch);

        List<FoundTargetDTO<TARGET_TYPE>> typedTargets = new();

        foreach (var item in candidatesToTarget)
        {
            TARGET_TYPE typedItem = item.GetComponent<TARGET_TYPE>();

            if (typedItem == null)
                continue;

            if (EqualityComparer<TARGET_TYPE>.Default.Equals(typedItem, queryData.ignoredTarget))
                continue;

            typedTargets.Add(new FoundTargetDTO<TARGET_TYPE>(typedItem, item.transform.position));
        }
        return typedTargets;
    }
}
