using UnityEngine;
using System.Collections.Generic;
public class TargetFinderCircularTest : MonoBehaviour
{
    public float radius = 1f;
    public ITargetFinder<IDamageReceiver, CircleTargetFinderQuerySettings> targetFinder;
    public CircleTargetFinderQuerySettings targetFinderQuerySettings;
    private void Awake()
    {
        targetFinder = new TargetFinder_CircularDIstance<IDamageReceiver>();
        targetFinderQuerySettings= new CircleTargetFinderQuerySettings(transform.position, radius);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }


    private void Update()
    {
        List<FoundTargetDTO<IDamageReceiver>> resultList = targetFinder.FindTargets(targetFinderQuerySettings, new List<IDamageReceiver>(), transform.forward);
        Debug.Log(resultList.Count);
    }
}
