using System.Collections.Generic;
using UnityEngine;

public class TargetFinder_BoxOverlap<TARGET_TYPE>
    : ITargetFinder<TARGET_TYPE, BoxTargetFinderQuerySettings<TARGET_TYPE>>
{
    public List<FoundTargetDTO<TARGET_TYPE>> FindTargets(BoxTargetFinderQuerySettings<TARGET_TYPE> queryData)
    {
        Collider2D[] candidates = Physics2D.OverlapBoxAll(
             queryData.center,
             queryData.size,
             queryData.angle,
             queryData.layersToSearch);

        HashSet<FoundTargetDTO<TARGET_TYPE>> foundTargets = new();

        foreach (Collider2D candidate in candidates)
        {
            TARGET_TYPE typedTarget = candidate.GetComponent<TARGET_TYPE>();

            if (typedTarget == null)
                continue;

            if (EqualityComparer<TARGET_TYPE>.Default.Equals(
                    typedTarget,
                    queryData.ignoredTarget))
            {
                continue;
            }

            foundTargets.Add(
                new FoundTargetDTO<TARGET_TYPE>(
                    typedTarget,
                    candidate.transform.position));
        }

        return new List<FoundTargetDTO<TARGET_TYPE>>(foundTargets);
    }
}

