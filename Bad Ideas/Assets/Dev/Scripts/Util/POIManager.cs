using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class POIManager : Singleton<POIManager>
{
    public List<POI> POIList { get; private set; }

    protected override void OnAwake()
    {
        POIList = new List<POI>();
    }

    public void AddPOI(POI poi)
    {
        POIList.Add(poi);
    }

    public Transform GetClosestPOI(Vector3 position, float distanceAllowed)
    {
        var filtered = POIList
            .Where(p => Vector3.Distance(position, p.transform.position) <= distanceAllowed)
            .OrderBy(p => p.InterestNumber)
            .ToList();

        return filtered.FirstOrDefault()?.transform;
    }
}
