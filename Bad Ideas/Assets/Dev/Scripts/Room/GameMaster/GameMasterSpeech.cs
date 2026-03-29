using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class GameMasterSpeech : MonoBehaviour
{
    [SerializedDictionary("<Dialogue Option, Text Index>", "UnityEvent")]
    [SerializeField] private SerializedDictionary<SerializedKeyValuePair<string, int>, UnityEvent> speechEvents;
    [SerializedDictionary("Dialogue Option", "Speech Sound")]
    [SerializeField] private SerializedDictionary<string, AudioClip> speechSounds;
    [SerializedDictionary("<Dialogue Option, Text Index>", "Speech Sound")]
    [SerializeField] private SerializedDictionary<SerializedKeyValuePair<string, int>, AudioClip> specificSpeechSounds;
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private RectTransform dialoguePanel;
    [SerializeField] private TextAsset dialogueTextFile;
    [SerializeField] private Vector2 outOfSightPosition;
    [SerializeField] private Vector2 onScreenPosition;

    [Space]
    [Header("Text/Speech")]
    [SerializeField] private float pitchMod;
    [SerializeField] private float CDMod;
    [SerializeField] private float maxPitchFlux;
    [SerializeField] private float dialogueCDFlux;
    [SerializeField] private float dialogueCooldown;
    [SerializeField] private float inbetweenCD = 2;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private string dialogueStart = "Beginning";
    private Dictionary<string, List<string>> Dialogues;
    private Coroutine dialogueCoroutine;
    private Spring textPanelSpring;
    private float textCooldown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        LoadText();

        PlayDialogue(dialogueStart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayDialogue(string dialogueName)
    {
        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }

        dialogueCoroutine = StartCoroutine(WriteText(dialogueName));
    }

    private IEnumerator WriteText(string name)
    {
        Coroutine textPanel = StartCoroutine(TextPanelAnimation(1));

        List<string> dialogue = Dialogues[name];

        for (int i = 0; i < dialogue.Count; i++)
        {
            UnityEvent ue;
            tmp.text = "";

            AudioClip clip = speechSounds[name];
            if (specificSpeechSounds.TryGetValue(new SerializedKeyValuePair<string, int>(name, i), out AudioClip specificClip))
                clip = specificClip;

            yield return StartCoroutine(StartType(dialogue[i], clip));

            if (speechEvents.TryGetValue(new SerializedKeyValuePair<string, int>(name, i), out ue))
            {
                ue?.Invoke();
            }
            yield return new WaitForSeconds(inbetweenCD);
        }

        StopCoroutine(textPanel);
        textPanel = StartCoroutine(TextPanelAnimation(0));
    }

    private IEnumerator StartType(string text, AudioClip clip)
    {
        string displayText = text;

        for (int i = 0; i < displayText.Length; i++)
        {
            char c = displayText[i];

            if (displayText[i] == '\\')
            {
                tmp.text += Environment.NewLine;
            }
            else
            {
                tmp.text += c;
                Speak(pitchMod, CDMod, clip);
            }

            yield return new WaitForSeconds(textCooldown);
        }
    }

    private void Speak(float pitchMod, float CDMod, AudioClip clip)
    {
        AudioSource current = audioSource;
        current.pitch = Mathf.Clamp(Random.Range(-maxPitchFlux, maxPitchFlux / 2) + pitchMod, .15f, 2);
        current.PlayOneShot(clip);

        textCooldown = dialogueCooldown + Random.Range(-dialogueCDFlux / 2, dialogueCDFlux) + CDMod;

    }

    private void LoadText()
    {
        textPanelSpring = new Spring(20, 0.8f, 0, true);
        Dialogues = new Dictionary<string, List<string>>();

        string[] text = dialogueTextFile.text.Split("\n");

        for (int i = 0; i < text.Length; i++)
        {
            string[] line = text[i].Trim().Split("\t");
            
            Dialogues.Add(line[0], line.ToList());
            Dialogues[line[0]].RemoveAt(0);
        }
    }

    private IEnumerator TextPanelAnimation(float position)
    {
        float timer = 5;

        textPanelSpring.RestPosition = position;

        while (timer > 0)
        {
            textPanelSpring.Update();
            dialoguePanel.anchoredPosition = Vector2.Lerp(outOfSightPosition, onScreenPosition, textPanelSpring.Position);

            timer -= Time.deltaTime;
            yield return null;
        }
    }
}
