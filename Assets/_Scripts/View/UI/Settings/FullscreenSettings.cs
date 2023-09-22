using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenSettings : MonoBehaviour
{
    [Header("UI Toggle")]
    [SerializeField] private Toggle fullscreenToggle;


    private void Start()
    {        
        LoadFullscreenSetting();
    }

    public void SetFullscreenSetting()
    {
        bool isOn = fullscreenToggle.isOn;
        Screen.fullScreen = isOn;

        PlayerPrefsRepository.Instance.SetFullscreenSetting(isOn);
    }

    private void LoadFullscreenSetting()
    {
        bool isOn = PlayerPrefsRepository.Instance.LoadFullscreenSetting();

        fullscreenToggle.isOn = isOn;

        SetFullscreenSetting();
    }
}
