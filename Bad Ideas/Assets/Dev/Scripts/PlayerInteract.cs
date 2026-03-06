using System;
using System.Collections;
using System.Collections.Generic;
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
    private RaycastHit hit;

    public CursorStatus CursorStatus {  get; private set; }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        RaycastHit hit;

        if (clickTimer < 0)
            CursorStatus = CursorStatus.None;

        if (Physics.Raycast(ray, out hit, stats.interactDistance, interactable))
        {
            this.hit = hit;

            if (clickTimer < 0)
                CursorStatus = CursorStatus.Hovering;
        }

        clickTimer -= Time.deltaTime;

        switch(CursorStatus)
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

        if (CursorStatus == CursorStatus.Hovering && clickTimer <= 0)
        {
            clickTimer = stats.clickTimer;
            CursorStatus = CursorStatus.Clicked;

            hit.collider.GetComponent<Interactable>()?.InteractWith();
        }
    }
}
