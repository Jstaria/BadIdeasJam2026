using UnityEngine;

public class TriggerBoxEndingTwo : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(this.transform);
    }
}
