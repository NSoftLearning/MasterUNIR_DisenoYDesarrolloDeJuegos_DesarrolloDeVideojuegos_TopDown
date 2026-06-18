using UnityEngine;

public class BoxTargetFinderQuerySettings <TARGET_TYPE>

{
    public LayerMask layersToSearch;
    public Vector2 center;
    public Vector2 size;
    public float angle;
    public TARGET_TYPE ignoredTarget;

    public BoxTargetFinderQuerySettings(
            LayerMask layersToSearch,
            Vector2 center,
            Vector2 size,
            float angle,
            TARGET_TYPE ignoredTarget)
    {
        this.layersToSearch = layersToSearch;
        this.center = center;
        this.size = size;
        this.angle = angle;
        this.ignoredTarget = ignoredTarget;
    }
}
