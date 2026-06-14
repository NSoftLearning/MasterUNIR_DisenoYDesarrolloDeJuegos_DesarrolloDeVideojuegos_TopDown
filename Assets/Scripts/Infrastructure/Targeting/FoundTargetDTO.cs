using UnityEngine;

public class FoundTargetDTO <TargetType>
{
    public TargetType target;
    public Vector3 position;

    public FoundTargetDTO (TargetType target, Vector3 position) 
    {
        this.target = target;
        this.position = position;
    }
}
