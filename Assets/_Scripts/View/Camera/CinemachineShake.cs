using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    private static string CAMERA_SHAKE = "CameraShake";

    public static CinemachineShake Instance { get; private set; }

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cbmcp;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;

    private bool isCameraShakeOn;

    private void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cbmcp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        LoadCameraShakeSettings();
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            cbmcp.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, (1 - (shakeTimer / shakeTimerTotal)));
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        if (!isCameraShakeOn)
        {
            return;
        }
        cbmcp.m_AmplitudeGain = intensity;

        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }

    public void LoadCameraShakeSettings()
    {
        if (PlayerPrefs.HasKey(CAMERA_SHAKE))
        {
            bool isOn = bool.Parse(PlayerPrefs.GetString(CAMERA_SHAKE));

            isCameraShakeOn = isOn;
        } else
        {
            isCameraShakeOn = true;
        }
    }

    public void SetCameraShakeSetting(bool cameraShakeOn)
    {
        isCameraShakeOn = cameraShakeOn;
    }
}
