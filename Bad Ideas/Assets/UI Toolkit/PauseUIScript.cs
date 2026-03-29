using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class PauseUIScript : MonoBehaviour
{
    private Button startButton;
    private Button settingsButton;
    private Button quitButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        startButton = root.Q<Button>("ResumeButton");
        settingsButton = root.Q<Button>("SettingsButton");
        quitButton = root.Q<Button>("QuitButton");

        startButton.RegisterCallback<ClickEvent>(OnStartButtonClicked);
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsButtonClicked);
        quitButton.RegisterCallback<ClickEvent>(OnQuitButtonClicked);
    }

    private void OnStartButtonClicked(ClickEvent clickEvent)
    {
        Debug.Log("Resuming Game");
        //SceneManager.LoadScene("_Name of Scene_");
    }
    private void OnSettingsButtonClicked(ClickEvent clickEvent)
    {
        Debug.Log("Opening Settings");
    }
    private void OnQuitButtonClicked(ClickEvent clickEvent)
    {
        Debug.Log("Quiting Game");
        Application.Quit();
    }

}
