using UnityEngine;
using System.Collections.Generic;
public struct CircleTargetFinderQuerySettings
{
    public Vector2 position;
    public float radius;
    public CircleTargetFinderQuerySettings(Vector2 position, float radius)
    {
        this.position = position;
        this.radius = radius;
    }
}
public class TargetFinder_CircularDIstance<TARGET_TYPE> : ITargetFinder<TARGET_TYPE, CircleTargetFinderQuerySettings>
{
    
    public List<FoundTargetDTO<TARGET_TYPE>> FindTargets(CircleTargetFinderQuerySettings circleTargetFinderQuerySettings, List<TARGET_TYPE> ignoreList, Vector3 originForward)
    {
        Collider2D[] colliderList = Physics2D.OverlapCircleAll(circleTargetFinderQuerySettings.position, circleTargetFinderQuerySettings.radius);
        List<FoundTargetDTO<TARGET_TYPE>> targetList = new List<FoundTargetDTO<TARGET_TYPE>>();
        TARGET_TYPE currentTarget;
        FoundTargetDTO<TARGET_TYPE> targetDTO;
        foreach (Collider2D col in colliderList)
        {
            
            currentTarget = col.gameObject.GetComponent<TARGET_TYPE>();
            if(currentTarget != null)
            {
                targetDTO = new FoundTargetDTO<TARGET_TYPE>(currentTarget, col.transform.position);
                targetList.Add(targetDTO);
            }
            
        }
        return targetList;
    }
}
