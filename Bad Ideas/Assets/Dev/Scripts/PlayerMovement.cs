using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController charCon;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private LayerMask ground;

    private Vector3 velocity, moveDampVelocity, forceVelocity;
    private bool onGround, isRunning;

    public bool IsGrounded => onGround;
    public bool IsRunning => isRunning;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();

        charCon.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        Ray ray = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(ray, 1.2f, ground))
        {
            forceVelocity.y = -2f;
            onGround = true;

            if (Input.GetKey(KeyCode.Space))
            {
                forceVelocity.y = playerStats.jumpForce;

                //// Add a little bit of movement direction to jumping
                //Vector3 lookDir = transform.forward;
                //lookDir.y = 0;
                //lookDir = lookDir.normalized;
                //Vector3 charVel = charCon.velocity;
                //charVel.y = 0;
                //charVel = charVel.normalized;

                //Vector3 moveDir = Vector3.Slerp(lookDir, charVel, .5f);

                //forceVelocity += moveDir.normalized;

                onGround = false;
            }
        }
        else
        {
            onGround = false;
            float gravityStrength = 9.81f * 2;
            forceVelocity.y -= gravityStrength * Time.deltaTime;
        }

        if (onGround)
        {
            forceVelocity = Vector3.Lerp(Vector3.up * -2, forceVelocity, Mathf.Pow(.5f, Time.deltaTime * 10));
        }

        charCon.Move(forceVelocity * Time.deltaTime);
    }

    private void Move()
    {
        Vector3 PlayerInput = new Vector3
         (
             Input.GetAxisRaw("Horizontal"),
             0f,
             Input.GetAxisRaw("Vertical")
         ).normalized;

        Vector3 moveVector = transform.TransformDirection(PlayerInput);

        bool ctrlPressed = Input.GetKey(KeyCode.LeftControl);
        isRunning = ctrlPressed;

        float CurrentSpeed = (ctrlPressed) ? playerStats.playerRunSpeed : playerStats.playerWalkSpeed;


        velocity = Vector3.SmoothDamp
        (
            velocity,
            moveVector * CurrentSpeed,
            ref moveDampVelocity,
            playerStats.moveSmoothTime
        );
    }
}
