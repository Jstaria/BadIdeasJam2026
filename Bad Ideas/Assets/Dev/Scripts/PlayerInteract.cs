using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum CursorStatus
{
    Hovering,
    Clicked,
    None
}

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private Camera playerCam;
    [SerializeField] private LayerMask interactable;
    [SerializeField] private Image cursor;

    private float clickTimer;
    private float hoverTimer;

    private RaycastHit hit;
    private Collider lastHitCollider; // for stability

    public CursorStatus CursorStatus { get; private set; }

    void Update()
    {
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);

        // SphereCast = more stable than Raycast
        bool didHit = Physics.SphereCast(ray, 0.05f, out hit, stats.interactDistance, interactable);

        clickTimer -= Time.deltaTime;
        hoverTimer -= Time.deltaTime;

        if (didHit)
        {
            // Only refresh timer if we're still on same object OR just hit something new
            if (hit.collider == lastHitCollider || hoverTimer <= 0)
            {
                hoverTimer = stats.clickTimer;
                lastHitCollider = hit.collider;
            }
        }
        else
        {
            // If we fully lose target, clear it after timer runs out
            if (hoverTimer <= 0)
            {
                lastHitCollider = null;
            }
        }

        // State priority
        if (clickTimer > 0)
        {
            CursorStatus = CursorStatus.Clicked;
        }
        else if (hoverTimer > 0 && lastHitCollider != null)
        {
            CursorStatus = CursorStatus.Hovering;
        }
        else
        {
            CursorStatus = CursorStatus.None;
        }

        UpdateCursor();
    }

    private void UpdateCursor()
    {
        switch (CursorStatus)
        {
            case CursorStatus.Hovering:
                cursor.sprite = stats.HoverSprite;
                break;

            case CursorStatus.Clicked:
                cursor.sprite = stats.ClickSprite;
                break;

            case CursorStatus.None:
                cursor.sprite = stats.CursorSprite;
                break;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (CursorStatus == CursorStatus.Hovering && clickTimer <= 0 && lastHitCollider != null)
        {
            clickTimer = stats.clickTimer;
            CursorStatus = CursorStatus.Clicked;

            lastHitCollider.GetComponent<Interactable>()?.InteractWith();
        }
    }
}