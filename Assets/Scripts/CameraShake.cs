using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float ShakeIntensity = 1.5f;
    private float duration = 1f;
    private bool shake = false;
    private float timer;
    private CinemachineBasicMultiChannelPerlin _cbmp;

    void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    void Start()
    {
        StopShakeCamera();
    }
    public void ShakeCamera()
    {
        _cbmp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        _cbmp.m_AmplitudeGain = ShakeIntensity;
        timer = duration;
    }

    public void StopShakeCamera()
    {
        _cbmp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        _cbmp.m_AmplitudeGain = 0f;
        timer = 0f;
    }

    void Update()
    {
        if (shake)
        {
            shake = false;
            ShakeCamera();
        }
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                StopShakeCamera();
            }
        }
    }

    public void SetShake(bool sh)
    {
        shake = sh;
    }
}
