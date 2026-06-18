using System;
using System.Collections.Generic;
using UnityEngine;

public class FoundTargetDTO <TARGET_TYPE> : IEquatable <FoundTargetDTO <TARGET_TYPE>>
{
    public TARGET_TYPE target;
    public Vector3 position;

    public FoundTargetDTO (TARGET_TYPE target, Vector3 position) 
    {
        this.target = target;
        this.position = position;
    }

    public bool Equals(FoundTargetDTO<TARGET_TYPE> other)
    {
        if (other == null) 
            return false;
        return EqualityComparer<TARGET_TYPE>.Default.Equals(target, other.target);
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as FoundTargetDTO<TARGET_TYPE>);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<TARGET_TYPE>.Default.GetHashCode(target);
    }
}
