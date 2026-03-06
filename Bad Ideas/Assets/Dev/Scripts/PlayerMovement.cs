using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController charCon;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private LayerMask ground;

    private Vector3 velocity;
    private Vector3 moveDampVelocity;
    private Vector3 forceVelocity;

    private Vector2 playerInput;

    private bool onGround;
    private bool isRunning;

    public bool IsGrounded => onGround;
    public bool IsRunning => isRunning;

    void Update()
    {
        GroundCheck();

        ApplyGravity();

        MovePlayer();
    }

    void GroundCheck()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        onGround = Physics.Raycast(ray, 1.2f, ground);

        if (onGround && forceVelocity.y < 0)
        {
            forceVelocity.y = -2f;
        }
    }

    void ApplyGravity()
    {
        forceVelocity.y += Physics.gravity.y * Time.deltaTime * 2;
    }

    void MovePlayer()
    {
        float speed = isRunning ? playerStats.playerRunSpeed : playerStats.playerWalkSpeed;

        Vector3 moveDir = transform.TransformDirection(new Vector3(playerInput.x, 0, playerInput.y));

        moveDir = Vector3.ClampMagnitude(moveDir, 1f);

        velocity = Vector3.SmoothDamp(
            velocity,
            moveDir * speed,
            ref moveDampVelocity,
            playerStats.moveSmoothTime
        );

        Vector3 finalMove = velocity + forceVelocity;

        charCon.Move(finalMove * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && onGround)
        {
            forceVelocity.y = playerStats.jumpForce;
        }
    }

    public void SetSprint(InputAction.CallbackContext context)
    {
        isRunning = context.control.IsPressed() && playerInput.sqrMagnitude > 0;
    }
}