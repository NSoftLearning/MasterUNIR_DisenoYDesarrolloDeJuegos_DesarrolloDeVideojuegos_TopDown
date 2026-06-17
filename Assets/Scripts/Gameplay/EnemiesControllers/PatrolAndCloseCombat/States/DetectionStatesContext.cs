using System.Collections.Generic;
using UnityEngine;

public struct DetectionStatesContext 
{
    public BasicTargetFinderQuerySettings basicTargetFindingQuerySettings;
    public ITargetFinder<IDamageReceiver, BasicTargetFinderQuerySettings> targetFinder;
    public IOrientationService orientationService;
    public IDamageReceiver objectToIgnore;

    public DetectionStatesContext(
        BasicTargetFinderQuerySettings basicTargetFindingQuerySettings, 
        ITargetFinder<IDamageReceiver, BasicTargetFinderQuerySettings> targetFinder, 
        IOrientationService orientationService, 
        IDamageReceiver damageReceiverToIgnore) 
    {
        this.basicTargetFindingQuerySettings = basicTargetFindingQuerySettings;
        this.targetFinder = targetFinder;
        this.orientationService = orientationService;
        this.objectToIgnore = damageReceiverToIgnore;
    }

}
