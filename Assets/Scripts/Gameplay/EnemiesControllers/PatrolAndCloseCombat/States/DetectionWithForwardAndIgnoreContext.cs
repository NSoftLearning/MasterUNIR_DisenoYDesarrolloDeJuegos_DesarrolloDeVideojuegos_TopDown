using System.Collections.Generic;
using UnityEngine;

public struct DetectionWithForwardAndIgnoreContext <SEARCHED_TYPE, SEARCH_QUERY_DATA> 
    where SEARCH_QUERY_DATA : IOrientedTargetFinderQuery, ITargetFinderWithIgnoreQuery<SEARCHED_TYPE>
{
    public SEARCH_QUERY_DATA querySettings;
    public ITargetFinder<SEARCHED_TYPE, SEARCH_QUERY_DATA> targetFinder;
    public IOrientationService orientationService;
    public SEARCHED_TYPE objectToIgnore;

    public DetectionWithForwardAndIgnoreContext(
        SEARCH_QUERY_DATA querySettings, 
        ITargetFinder<SEARCHED_TYPE, SEARCH_QUERY_DATA> targetFinder, 
        IOrientationService orientationService,
        SEARCHED_TYPE damageReceiverToIgnore) 
    {
        this.querySettings = querySettings;
        this.targetFinder = targetFinder;
        this.orientationService = orientationService;
        this.objectToIgnore = damageReceiverToIgnore;
    }
    public SEARCH_QUERY_DATA GetCurrentQueryData()
    {
        SEARCH_QUERY_DATA currentQueryData = querySettings;
        currentQueryData.OriginForward = orientationService.Forward;
        return currentQueryData;
    }


}
