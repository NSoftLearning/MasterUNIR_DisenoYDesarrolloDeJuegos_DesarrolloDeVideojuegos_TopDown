using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetFinder <TARGET_TYPE, QUERY_DATA>
{
    public List<FoundTargetDTO<TARGET_TYPE>> FindTargets(QUERY_DATA queryData);
    
}
