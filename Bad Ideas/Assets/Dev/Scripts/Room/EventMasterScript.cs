using AYellowpaper.SerializedCollections;
using EasyDoorSystem;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventMasterScript : MonoBehaviour
{
    [SerializedDictionary("Name","Animator")]
    public SerializedDictionary<string, Animator> animators;

    [SerializedDictionary("Name", "Transforms")]
    public SerializedDictionary<string, Transform> transforms;

    [SerializedDictionary("Name", "Doors")]
    public SerializedDictionary<string, EasyDoor> doors;

    [SerializeField] private MyButton button;
    [SerializeField] private GameMasterSpeech speech;

    private Dictionary<string, UnityEvent> events;

    public void Awake()
    {
        events = new();
        events.Add("EnterRoom", new UnityEvent());
        events["EnterRoom"].AddListener(EnterRoom);

        events.Add("BeginPuzzles", new UnityEvent());
        events["BeginPuzzles"].AddListener(BeginPuzzles);
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

        speech.PlayDialogue("PressButton");

        button.OnButtonPress = new();
        button.OnButtonPress.AddListener(() => StartEvent("BeginPuzzles"));
    }


    public void BeginPuzzles()
    {
        StartCoroutine(BegingPuzzlesCoroutine());
    }

    private IEnumerator BegingPuzzlesCoroutine()
    {
        doors["curtains"].OpenDoor();

        yield return WaitForSeconds(2f);

        doors["door1"].OpenDoor();
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
