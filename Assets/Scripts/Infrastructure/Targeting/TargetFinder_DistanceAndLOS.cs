using System.Collections.Generic;

using UnityEngine;

public class TargetFinder_DistanceAndLOS<TARGET_TYPE> : ITargetFinder<TARGET_TYPE, BasicTargetFinderQuerySettings>
{ 

    public List<FoundTargetDTO<TARGET_TYPE>> FindTargets(BasicTargetFinderQuerySettings queryData, List<TARGET_TYPE> ignoreList, Vector3 originForward)
    {
        List<FoundTargetDTO<TARGET_TYPE>> typedTargetsCandidateList =  GetTypedTargetsByDistance(queryData, ignoreList);
        List<FoundTargetDTO<TARGET_TYPE>> typedTargetsFinalList = new List<FoundTargetDTO<TARGET_TYPE>>();

        foreach (FoundTargetDTO<TARGET_TYPE> item in typedTargetsCandidateList)
        {
            Vector3 fromOriginToTarget = item.position - queryData.origintransform.position;
            fromOriginToTarget.z = 0;
            originForward.z = 0;

            float signedAngle = Vector3.SignedAngle(originForward, fromOriginToTarget, Vector3.forward);
            if (Mathf.Abs(signedAngle) <= queryData.halfFieldOfView)
                typedTargetsFinalList.Add(item);

            //Debug.DrawLine(queryData.origintransform.position, item.position, Color.red, .1f);
        }

        


        return typedTargetsFinalList;
    }

    List<FoundTargetDTO<TARGET_TYPE>> GetTypedTargetsByDistance(BasicTargetFinderQuerySettings queryData, List<TARGET_TYPE> ignoreList)
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
                typedTargets.Add(new FoundTargetDTO<TARGET_TYPE>(typedItem, item.transform.position));
            }
        }
        return typedTargets;
    }
}
