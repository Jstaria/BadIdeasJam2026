using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFollow : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private PlayerMovement player;


    public Vector3 Offset { get { return offset; } set { offset = value; } }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.IsGrounded)
        {
            transform.position = Target.position + offset;
        }
    }
}
