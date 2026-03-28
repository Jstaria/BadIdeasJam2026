using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UsingButtons : MonoBehaviour
{
    private Button resumeButton;
    private Button restartButton;
    private Button settingsButton;
    private Button quitButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        resumeButton = root.Q<Button>("ResumeButton");
        restartButton = root.Q<Button>("RestartButton");
        settingsButton = root.Q<Button>("SettingsButton");
        quitButton = root.Q<Button>("QuitButton");

        resumeButton.RegisterCallback<ClickEvent>(OnResumeButtonClicked);
        restartButton.RegisterCallback<ClickEvent>(OnRestartButtonClicked);
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsButtonClicked);
        quitButton.RegisterCallback<ClickEvent>(OnQuitButtonClicked);
    }

    
    private void OnResumeButtonClicked(ClickEvent clickEvent)
    {
        Debug.Log("Resuming Game");
    }
    private void OnRestartButtonClicked(ClickEvent clickEvent)
    {
        Debug.Log("Restarting Level");
    }
    private void OnSettingsButtonClicked(ClickEvent clickEvent)
    {
        Debug.Log("Opening Settings");
    }
    private void OnQuitButtonClicked(ClickEvent clickEvent)
    {
        Debug.Log("Quiting Game");
    }

}
