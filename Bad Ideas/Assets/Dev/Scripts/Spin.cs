using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float rotationSPD = 1;
    private Vector3 axis;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        axis = Random.onUnitSphere;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axis, rotationSPD * Time.deltaTime);
    }
}
