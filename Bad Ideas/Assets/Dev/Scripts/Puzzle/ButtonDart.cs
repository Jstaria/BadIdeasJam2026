using UnityEngine;

public class ButtonDart : MonoBehaviour
{
    [SerializeField] Interactable inter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dart"))
        {
            inter.OnInteract?.Invoke();
        }
    }
}
