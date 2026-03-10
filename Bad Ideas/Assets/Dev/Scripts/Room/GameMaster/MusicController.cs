using AYellowpaper.SerializedCollections;
using Seagull.Interior_I1.Inspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicController : Singleton<MusicController>
{
    [Header("Speakers")]
    public List<SpeakerCon> speakers;

    [Space]
    [Header("Speaker Audio")]
    [SerializedDictionary("Song BPM, Song")]
    [SerializeField] private SerializedDictionary<int, AudioClip> songs;
    [SerializeField] private AudioClip transition;

    private AudioSource speaker;
    private int songIndex = 0;
    private Coroutine songCoroutine;

    protected override void OnAwake()
    {
        speaker = GetComponent<AudioSource>();

        StartSpeakers();
    }

    private IEnumerator PlayMusic()
    {
        songIndex = GetSongIndex();
        speaker.clip = songs.Values.ToList()[songIndex];

        speaker.PlayOneShot(transition);
        speaker.Play();

        while (true)
        {
            if (speaker.time / speaker.clip.length > .99f)
            {
                StartCoroutine(ResetSpeakers());
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator ResetSpeakers()
    {
        StopSpeakers();
        StartSpeakers();

        yield return new WaitForFixedUpdate();
    }

    public int GetSongIndex()
    {
        int index;

        do
        {
            index = Random.Range(0, songs.Count);
        }
        while (index == songIndex);

        return index;
    }

    public void StartSpeakers()
    {
        songCoroutine = StartCoroutine(PlayMusic());

        for (int i = 0; i < speakers.Count; i++)
        {
            speakers[i].StartGrooving(songs.Keys.ToList()[songIndex]);
        }
    }

    public void StopSpeakers()
    {
        if (songCoroutine == null) return;

        for (int i = 0; i < speakers.Count; i++)
        {
            speakers[i].StopGrooving();
        }

        StopCoroutine(songCoroutine);
        songCoroutine = null;

        speaker.Stop();
    }


}