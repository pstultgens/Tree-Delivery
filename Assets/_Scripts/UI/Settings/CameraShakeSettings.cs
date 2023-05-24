using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraShakeSettings : MonoBehaviour
{
    [Header("UI Sliders")]
    [SerializeField] private Toggle cameraShakeToggle;

    private CinemachineShake cinemachineShake;

    private void Start()
    {
        cinemachineShake = FindObjectOfType<CinemachineShake>();
        LoadCameraShakeSetting();
    }

    public void SetCameraShakeSetting()
    {
        bool isOn = cameraShakeToggle.isOn;

        cinemachineShake.SetCameraShakeSetting(isOn);
        PlayerPrefsRepository.Instance.SetCameraShakeSetting(isOn);
    }

    private void LoadCameraShakeSetting()
    {
        bool isOn = PlayerPrefsRepository.Instance.LoadCameraShakeSetting();

        cameraShakeToggle.isOn = isOn;
        cinemachineShake.SetCameraShakeSetting(isOn);

        SetCameraShakeSetting();
    }
}
