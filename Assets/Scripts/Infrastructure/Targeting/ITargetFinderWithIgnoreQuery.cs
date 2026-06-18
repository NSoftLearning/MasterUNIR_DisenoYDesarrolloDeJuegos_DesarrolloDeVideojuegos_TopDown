using UnityEngine;

public interface ITargetFinderWithIgnoreQuery <TARGET_TYPE>
{
    TARGET_TYPE IgnoredTarget { get; set; } 
}
