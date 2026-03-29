using UnityEngine;
using UnityEngine.Rendering;

public class Block : MonoBehaviour
{
    [SerializeField] private PlayerPickup playerPickup;
    [SerializeField] private Interactable interactable;

    public bool pickedUp = false;

    public void SetPlayerPickup()
    {
        if (!pickedUp)
        {
            if (TryGetComponent(out Rigidbody rb))
                playerPickup.connectedRB = rb;

            playerPickup.OnPickup();
            pickedUp = true;
        }
        else
        {
            playerPickup.OnDrop();
            pickedUp = false;
        }

    }
}
