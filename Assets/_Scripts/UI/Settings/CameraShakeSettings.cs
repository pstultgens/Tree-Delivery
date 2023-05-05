using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraShakeSettings : MonoBehaviour
{
    private static string CAMERA_SHAKE = "CameraShake";

    [Header("UI Sliders")]
    [SerializeField] private Toggle cameraShakeToggle;

    private CinemachineShake cinemachineShake;

    private void Start()
    {
        cinemachineShake = FindObjectOfType<CinemachineShake>();

        if (PlayerPrefs.HasKey(CAMERA_SHAKE))
        {
            LoadCameraShakeSetting();
        }
        else
        {
            SetCameraShakeSetting();
        }
    }

    public void SetCameraShakeSetting()
    {
        bool isOn = cameraShakeToggle.isOn;
        Debug.Log("CameraShakeSettings: Set Camera Shake: " + isOn + " in PlayerPrefs");

        cinemachineShake.SetCameraShakeSetting(isOn);

        PlayerPrefs.SetString(CAMERA_SHAKE, isOn.ToString());
    }

    private void LoadCameraShakeSetting()
    {
        bool isOn = bool.Parse(PlayerPrefs.GetString(CAMERA_SHAKE));
        Debug.Log("CameraShakeSettings: Load Camera Shake: " + isOn + " from PlayerPrefs");
        cameraShakeToggle.isOn = isOn;
        cinemachineShake.SetCameraShakeSetting(isOn);
        
        SetCameraShakeSetting();
    }
}
