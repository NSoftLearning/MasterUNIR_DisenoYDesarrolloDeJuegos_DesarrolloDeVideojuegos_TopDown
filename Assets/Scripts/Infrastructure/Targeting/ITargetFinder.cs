using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetFinder <TARGET_TYPE>
{
    public List<FoundTargetDTO<TARGET_TYPE>> FindTargets(TargetFinderDataDTO queryData);
    
}
