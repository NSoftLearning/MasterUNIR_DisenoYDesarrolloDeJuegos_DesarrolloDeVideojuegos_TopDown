using System.Collections.Generic;
using UnityEngine;

public struct DetectionStatesContext <SEARCHED_TYPE, SEARCH_QUERY_DATA>
{
    public SEARCH_QUERY_DATA basicTargetFindingQuerySettings;
    public ITargetFinder<SEARCHED_TYPE, SEARCH_QUERY_DATA> targetFinder;
    public IOrientationService orientationService;
    public IDamageReceiver objectToIgnore;

    public DetectionStatesContext(
        SEARCH_QUERY_DATA basicTargetFindingQuerySettings, 
        ITargetFinder<SEARCHED_TYPE, SEARCH_QUERY_DATA> targetFinder, 
        IOrientationService orientationService, 
        IDamageReceiver damageReceiverToIgnore) 
    {
        this.basicTargetFindingQuerySettings = basicTargetFindingQuerySettings;
        this.targetFinder = targetFinder;
        this.orientationService = orientationService;
        this.objectToIgnore = damageReceiverToIgnore;
    }

}
