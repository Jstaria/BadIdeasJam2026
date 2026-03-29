using UnityEngine;

public class Balloon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Dart dart))
        {
            Destroy(gameObject);
        }
    }
}
