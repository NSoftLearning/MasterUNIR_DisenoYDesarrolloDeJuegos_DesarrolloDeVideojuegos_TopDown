using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetFinder <TARGET_TYPE, PARAMETER_TYPE>
{
    public List<FoundTargetDTO<TARGET_TYPE>> FindTargets(PARAMETER_TYPE queryData, TARGET_TYPE ignore, Vector3 originForward);
    
}
