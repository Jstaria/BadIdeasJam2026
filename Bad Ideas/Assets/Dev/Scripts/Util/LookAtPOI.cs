using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LookAtPOI : MonoBehaviour
{
    [SerializeField] private float detectionRange;
    [SerializeField] private MultiAimConstraint constraint;
    [SerializeField] private Transform sourceOBJ;

    private Vector3 targetPosition;
    [SerializeField] private Vector3 defaultPositionOffset;

    private void Update()
    {
        Transform tr = POIManager.Instance.GetClosestPOI(transform.position, detectionRange);

        if (tr == null)
            targetPosition = transform.localPosition + defaultPositionOffset;
        else
            targetPosition = tr.position;


            sourceOBJ.position = Vector3.Slerp(targetPosition, sourceOBJ.position, Mathf.Pow(.5f, Time.deltaTime * 10));
    }
}
