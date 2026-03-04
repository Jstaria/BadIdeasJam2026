using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoShakeCamera : MonoBehaviour
{
    [SerializeField] private float strength = 10;
    [SerializeField] private float angularFrequency = 10;
    [SerializeField] private float dampingRatio = .5f;

    private void Awake()
    {
        CameraShake.Instance.ShakeCameraRandom(angularFrequency, dampingRatio, strength);    
    }
}
