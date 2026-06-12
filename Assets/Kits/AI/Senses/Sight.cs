using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    public List<IVisible> visiblesInSight = new List <IVisible>();
    [SerializeField] private float radius = 3f;
    [SerializeField] List<IVisible.Side> attendedSides;

    private void Update()
    {
        visiblesInSight.Clear();

        Collider2D[] potentialVisibles = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D visible in potentialVisibles)
        {
            IVisible newVisible = visible.GetComponent<IVisible>();
            if ((newVisible != null) &&
                attendedSides.Contains(newVisible.GetSide()))
                visiblesInSight.Add(newVisible);
        }
    }
}
