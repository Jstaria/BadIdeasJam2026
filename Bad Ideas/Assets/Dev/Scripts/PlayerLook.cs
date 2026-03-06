using QFSW.QC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private AnimationCurve mouseSensitivityCurve;

    private Vector2 XYRotation;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        XYRotation = transform.localRotation.eulerAngles;
    }

    // Update is called once per frame
    public void OnMouseMove(InputAction.CallbackContext context)
    {
        if (QuantumConsole.Instance.IsActive /*|| Menu.Instance.IsPaused*/)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Cursor.lockState = QuantumConsole.Instance.IsActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = QuantumConsole.Instance.IsActive;

        Vector2 MouseInput = context.ReadValue<Vector2>();

        XYRotation.x -= MouseInput.y * mouseSensitivityCurve.Evaluate(playerStats.sensitivity.y);
        XYRotation.y += MouseInput.x * mouseSensitivityCurve.Evaluate(playerStats.sensitivity.x);

        XYRotation.x = Mathf.Clamp(XYRotation.x, -75f, 75f);

        transform.eulerAngles = new Vector3(0f, XYRotation.y);
        PlayerCamera.localEulerAngles = new Vector3(XYRotation.x, 0f);
    }
}
