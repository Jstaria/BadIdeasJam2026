using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POI : MonoBehaviour
{
    [SerializeField] public int InterestNumber = 10;

    private void Awake()
    {
        POIManager.Instance.AddPOI(this);
    }
}
