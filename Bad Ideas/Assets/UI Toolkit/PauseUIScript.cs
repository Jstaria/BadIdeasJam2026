using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using Cursor = UnityEngine.Cursor;
using System.Threading;

public class PauseUIScript : MonoBehaviour
{
    [SerializeField] private GameObject root;
    private bool isPaused;

    void Awake()
    {
        root.SetActive(false);
    }

    public void TogglePause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        isPaused = !isPaused;

        if (!isPaused)
        {
            Time.timeScale = 1f;
            root.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Time.timeScale = 0f;
            root.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        Debug.Log("Toggling Pause");
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (!isPaused)
        {
            Time.timeScale = 1f;
            root.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Time.timeScale = 0f;
            root.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        Debug.Log("Toggling Pause");
    }

    public void OnResumeButtonClicked()
    {
        Debug.Log("Resuming Game");
        TogglePause();
    }

    public void OnRestartButtonClicked()
    {
        Debug.Log("Restarting Scene");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnQuitButtonClicked()
    {
        Debug.Log("Quiting Game");



            Application.Quit();

    }
}