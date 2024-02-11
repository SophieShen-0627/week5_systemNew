using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private float ShakeDuration = 0.3f; 
    private float ShakeAmplitude = 1.2f;     //·ù¶È
    private float ShakeFrequency = 2.0f;     //ÆµÂÊ

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;

    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();

        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
    cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0;
    }

    public void StartShake(float Duration, float Amplitude, float Frequency)
    {
        ShakeDuration = Duration;
        ShakeAmplitude = Amplitude;
        ShakeFrequency = Frequency;

        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = ShakeAmplitude;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = ShakeFrequency;
        shakeTimer = ShakeDuration;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
                cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0f;
            }
        }
    }
}
