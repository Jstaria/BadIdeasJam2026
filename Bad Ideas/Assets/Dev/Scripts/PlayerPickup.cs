using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private PlayerLook playerLook;
    [SerializeField] private Transform playerPickupObj;
    [SerializeField] private LayerMask interactable;
    [SerializeField] private SpringJoint joint;

    public Rigidbody connectedRB;

    public bool HoldingSomething => connectedRB != null;

    // Start is called before the first frame update
    void Start()
    {
        connectedRB = null;
    }

    // Update is called once per frame
    void Update()
    {
        //playerPickupObj.position = transform.position + Camera.main.transform.forward * stats.interactDistance;
    }

    public void OnPickup()
    {
        Debug.Log("Clicked");
        joint.connectedBody = connectedRB;
        //connectedRB.freezeRotation = true;
    }

    public void OnDrop()
    {
        //connectedRB.freezeRotation = false;
        joint.connectedBody = null;
        connectedRB = null;
    }
}
