using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetFinder <TARGET_TYPE, PARAMETER_TYPE>
{
    public List<FoundTargetDTO<TARGET_TYPE>> FindTargets(PARAMETER_TYPE queryData, List<TARGET_TYPE> ignoreList, Vector3 originForward);
    
}
