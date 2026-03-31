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
    private Coroutine fiveMinTimer;

    public void Awake()
    {
        events = new();
        events.Add("EnterRoom", new UnityEvent());
        events["EnterRoom"].AddListener(EnterRoom);

        events.Add("StartPuzzle1", new UnityEvent());
        events["StartPuzzle1"].AddListener(BeginPuzzles);

        events.Add("StartPuzzle2", new UnityEvent());
        events["StartPuzzle2"].AddListener(StartPuzzle2);

        events.Add("StartPuzzle3", new UnityEvent());
        events["StartPuzzle3"].AddListener(StartPuzzle3);

        events.Add("StartPuzzle4", new UnityEvent());
        events["StartPuzzle4"].AddListener(StartPuzzle4);
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

        fiveMinTimer = StartCoroutine(FiveMinTimer("5minTimer"));

        button.OnButtonPress = new();
        button.OnButtonPress.AddListener(() => StartEvent("StartPuzzle2"));
    }

    public void StartPuzzle2()
    {
        StartCoroutine(StartPuzzle2Coroutine());
    }

    private IEnumerator StartPuzzle2Coroutine()
    {
        speech.PlayDialogue("StartPuzzle2");

        yield return SpinCarousel();

        doors["door2"].OpenDoor();

        if (fiveMinTimer != null)
        {
            StopCoroutine(fiveMinTimer);
        }

        fiveMinTimer = StartCoroutine(FiveMinTimer("5minTimer2"));

        button.OnButtonPress = new();
        button.OnButtonPress.AddListener(() => StartEvent("StartPuzzle3"));
    }

    public void StartPuzzle3()
    {
        StartCoroutine(StartPuzzle3Coroutine());
    }

    private IEnumerator StartPuzzle3Coroutine()
    {
        speech.PlayDialogue("StartPuzzle3");

        yield return SpinCarousel();

        doors["door3"].OpenDoor();

        if (fiveMinTimer != null)
        {
            StopCoroutine(fiveMinTimer);
        }

        fiveMinTimer = StartCoroutine(FiveMinTimer("5minTimer3"));

        button.OnButtonPress = new();
        button.OnButtonPress.AddListener(() => StartEvent("StartPuzzle4"));
    }
    public void StartPuzzle4()
    {
        StartCoroutine(StartPuzzle4Coroutine());
    }
    private IEnumerator StartPuzzle4Coroutine()
    {
        if (fiveMinTimer != null)
        {
            StopCoroutine(fiveMinTimer);
        }

        speech.PlayDialogue("StartPuzzle4");
        yield return SpinCarousel();
        doors["door4"].OpenDoor();

        yield return StartCoroutine(FiveMinTimer("5minTimer4", 20));

        yield return StartCoroutine(FiveMinTimer("Puzzle4Failed", 10));
    }

    public void StopCo()
    {
        StopAllCoroutines();
    }

    private IEnumerator FiveMinTimer(string timerName, int actualTime = 300)
    {
        yield return WaitForSeconds(actualTime);

        speech.PlayDialogue(timerName);
    }

    public void SecondEnding()
    {
        StartCoroutine(SpinCarousel());
        speech.PlayDialogue("DartAtButton");

        transforms["dart"].gameObject.SetActive(true);
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
