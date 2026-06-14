using UnityEngine;

public class FoundTargetDTO <TargetType>
{
    public TargetType target;
    //public Transform transform;

    public FoundTargetDTO (TargetType target /*, Transform transform*/) 
    {
        this.target = target;
      //  this.transform = transform;
    }
}
