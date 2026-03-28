
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteAlways]
public class ArrayMod : MonoBehaviour
{
    [SerializeField] private SplineContainer ArraySpline;
    [SerializeField] private GameObject ArrayPrefab;
    [SerializeField] private int NumberOfObjects = 1;
    [SerializeField] private Transform LookAtTransform;
    [SerializeField] private Vector3 rotationOffset;

    private List<GameObject> arrayObjects;

    // Update is called once per frame
    void Update()
    {
        if (arrayObjects == null) arrayObjects = new List<GameObject>();
        if (arrayObjects.Count != NumberOfObjects)
        {
            for (int i = 0; i < arrayObjects.Count; i++)
            {
                DestroyImmediate(arrayObjects[i]);
            }
            arrayObjects.Clear();
            for (int i = 0; i < NumberOfObjects; i++)
            {
                GameObject newObj = Instantiate(ArrayPrefab, transform);
                arrayObjects.Add(newObj);
            }
        }

        for (int i = 0; i < arrayObjects.Count; i++)
        {
            if (LookAtTransform != null)
            {
                arrayObjects[i].transform.LookAt(LookAtTransform.position);
            }

            arrayObjects[i].transform.position = ArraySpline.EvaluatePosition(i / (float)NumberOfObjects);
            arrayObjects[i].transform.Rotate(rotationOffset);
        }
    }
}
