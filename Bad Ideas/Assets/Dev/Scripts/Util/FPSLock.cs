using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLock : Singleton<FPSLock>
{
    [SerializeField] private bool lockFps;
    [SerializeField] private bool useVSync;
    [SerializeField] private int targetFPS;

    protected override void OnAwake()
    {
        CheckValues();

        base.OnAwake();
    }

    private void Update()
    {
        CheckValues();
    }

    void CheckValues()
    {
        Application.targetFrameRate = lockFps ? targetFPS : -1;

        QualitySettings.vSyncCount = useVSync ? 1 : 0;
    }
}
