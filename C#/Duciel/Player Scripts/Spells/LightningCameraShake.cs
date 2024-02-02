using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningCameraShake : MonoBehaviour
{
    [SerializeField] CinemachineImpulseSource _screenShake;
    [SerializeField] float shakePower;

    private void Awake()
    {
        ScreenShake();
    }

    public void ScreenShake()
    {
        _screenShake.GenerateImpulse();
    }
}
