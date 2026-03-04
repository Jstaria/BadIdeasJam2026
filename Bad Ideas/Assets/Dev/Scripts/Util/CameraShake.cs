using IdleEngine;
using Mono.CSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    private Spring2D shakeSpring;
    private Vector3 position;

    protected override void OnAwake()
    {
        position = transform.localPosition;
        shakeSpring = new Spring2D(0, 0, transform.localPosition);
    }

    private void Update()
    {
        shakeSpring.Update();
    }

    private void LateUpdate()
    {
        transform.localPosition = shakeSpring.Position;
    }

    public void ShakeCamera(float angularFrequency, float dampingRatio, Vector2 nudge)
    {
        shakeSpring.SetValues(angularFrequency, dampingRatio);
        shakeSpring.Nudge(nudge);
    }

    public void ShakeCameraRandom(float angularFreq, float dampingRatio, float str)
    {
        shakeSpring.SetValues(angularFreq, dampingRatio);
        shakeSpring.Nudge(Random.insideUnitCircle * str);
    }
}
