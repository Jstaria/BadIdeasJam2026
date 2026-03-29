using EasyDoorSystem;
using UnityEngine;

public class Chute : MonoBehaviour
{
    [SerializeField] private EasyDoor door;
    [SerializeField] private string Tag;
    [SerializeField] private BlockSimon bs;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Block b))
        {
            if (!b.pickedUp) return;
            if (!b.CompareTag(Tag)) return;

            door.OpenDoor();
            // Play kerthunk sound here
            bs.RemoveBlock(b.gameObject);
        }
    }
}
