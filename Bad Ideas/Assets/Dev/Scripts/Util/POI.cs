using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POI : MonoBehaviour
{
    [SerializeField] public int InterestNumber = 10;

    private void Awake()
    {
        StartCoroutine(AddPOI());
    }

    private IEnumerator AddPOI()
    {
        yield return new WaitForSeconds(2);
        POIManager.Instance.AddPOI(this);
    }
}
