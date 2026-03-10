using Ionic.Zip;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpeakerCon : MonoBehaviour
{
    [Header("Speaker Animation")]
    [SerializeField] private float stretchAmount = 1.5f;  
    [SerializeField] private float squashAmount = 0.5f;   
    [SerializeField] private float animationSpeed = 2f;
    [SerializeField] private AnimationCurve grooveCurve;
    [SerializeField] private float animationOffset;

    [SerializeField] private List<SpeakerCon> subCons;

    private Vector3 originalScale;
    private Coroutine grooveCoroutine;

    private int songIndex;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private IEnumerator GrooveCoroutine(int bpm)
    {
       float secondsPerBeat = 60f / bpm;

    while (true)
    {
        float beatProgress = (Time.time / secondsPerBeat) % 1f;

        float progress = grooveCurve.Evaluate((beatProgress * animationSpeed + animationOffset) % 1f);
        float progressCos = grooveCurve.Evaluate((beatProgress * animationSpeed + 0.5f + animationOffset) % 1f);

        float factor = Mathf.Lerp(1f, stretchAmount, progress);
        float factorCos = Mathf.Lerp(1f, squashAmount, progressCos);

        transform.localScale = new Vector3(
            originalScale.x * factorCos,
            originalScale.y * factor,
            originalScale.z * factorCos
        );

        yield return new WaitForFixedUpdate();
    }
    }

    public void StopGrooving()
    {
        StopCoroutine(grooveCoroutine);
        grooveCoroutine = null;

        foreach (var con in subCons)
        {
            con.StopGrooving();
        }
    }

    public void StartGrooving(int bpm)
    {
        if (originalScale == Vector3.zero)
            originalScale = transform.localScale;

        grooveCoroutine = StartCoroutine(GrooveCoroutine(bpm));

        foreach (var con in subCons)
        {
            con.StartGrooving(bpm);
        }
    }
}
