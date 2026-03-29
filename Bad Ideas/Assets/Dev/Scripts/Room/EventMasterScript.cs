using AYellowpaper.SerializedCollections;
using EasyDoorSystem;
using IKVM.Reflection;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class EventMasterScript : MonoBehaviour
{
    [SerializedDictionary("Name", "Animator")]
    public SerializedDictionary<string, Animator> animators;

    [SerializedDictionary("Name", "Transforms")]
    public SerializedDictionary<string, Transform> transforms;

    [SerializedDictionary("Name", "Doors")]
    public SerializedDictionary<string, EasyDoor> doors;

    [SerializeField] private MyButton button;
    [SerializeField] private GameMasterSpeech speech;

    [SerializeField] private PlayerMovement playerMovement;

    private Dictionary<string, UnityEvent> events;

    public void Awake()
    {
        events = new();
        events.Add("EnterRoom", new UnityEvent());
        events["EnterRoom"].AddListener(EnterRoom);

        events.Add("StartPuzzle1", new UnityEvent());
        events["StartPuzzle1"].AddListener(BeginPuzzles);

        events.Add("StartPuzzle2", new UnityEvent());
        events["StartPuzzle2"].AddListener(StartPuzzle2);
    }

    public void EnterRoom()
    {
        StartCoroutine(EnterRoomCoroutine());
    }

    private IEnumerator EnterRoomCoroutine()
    {
        animators["door"].SetTrigger("Open");

        button.isDisabled = true;

        yield return WaitForSeconds(3f);

        speech.PlayDialogue("FIrstButtonPress");

        button.OnButtonPress = new();
        button.OnButtonPress.AddListener(() => StartEvent("StartPuzzle1"));
    }


    public void CheckForMouseMovement()
    {
        StartCoroutine(CheckMouse());
    }

    private IEnumerator CheckMouse()
    {
        bool mouseMoved = false;

        while (!mouseMoved)
        {
            mouseMoved = Mouse.current.delta.ReadValue() != Vector2.zero;
            yield return null;
        }

        speech.PlayDialogue("MouseMovement");
    }

    public void CheckForWASD()
    {
        StartCoroutine(CheckWASD());
    }

    private IEnumerator CheckWASD()
    {
        bool moved = false;

        while (!moved)
        {
            moved = playerMovement.Moved;

            yield return null;
        }

        speech.PlayDialogue("WASDinput");
    }

    public void BeginPuzzles()
    {
        StartCoroutine(BegingPuzzlesCoroutine());
    }

    private IEnumerator BegingPuzzlesCoroutine()
    {
        doors["curtains"].OpenDoor();

        yield return WaitForSeconds(1f);

        doors["door1"].OpenDoor();

        speech.PlayDialogue("StartPuzzle1");

        button.OnButtonPress = new();
        button.OnButtonPress.AddListener(() => StartEvent("StartPuzzle2"));
    }

    public void StartPuzzle2()
    {
        StartCoroutine(StartPuzzle2Coroutine());
    }

    private IEnumerator StartPuzzle2Coroutine()
    {
        yield return SpinCarousel();

        doors["door2"].OpenDoor();

        button.OnButtonPress = new();
        button.OnButtonPress.AddListener(() => StartEvent("StartPuzzle3"));
    }

    private IEnumerator SpinCarousel()
    {
        Transform t = transforms["carousel"];

        Quaternion start = t.rotation;
        Quaternion target = start * Quaternion.Euler(0, 90, 0);

        float time = 0f;
        float duration = 2f;

        while (time < duration)
        {
            t.rotation = Quaternion.Slerp(start, target, time / duration);

            time += Time.deltaTime;
            yield return null;
        }

        t.rotation = target;
    }

    #region // Supporting Methods
    private IEnumerator WaitForSeconds(float seconds)
    {
        float elapsedTime = 0f;
        while (elapsedTime < seconds)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void StartEvent(string name)
    {
        if (events.ContainsKey(name))
        {
            events[name]?.Invoke();
        }
        else
        {
            Debug.LogWarning($"Event '{name}' not found.");
        }
    }
    #endregion
}
