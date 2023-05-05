using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    private static string MUSIC_VOLUME = "MusicVolume";
    private static string SFX_VOLUME = "SFXVolume";

    [Header("Audio Mixers")]
    [SerializeField] public AudioMixer audioMixer;

    [Header("UI Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey(MUSIC_VOLUME))
        {
            LoadMusicVolume();
        }
        else
        {
            SetMusicVolume();
        }

        if (PlayerPrefs.HasKey(SFX_VOLUME))
        {
            LoadSFXVolume();
        }
        else
        {
            SetSFXVolume();
        }
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        Debug.Log("VolumeSettings: Set Music Volume: " + volume + " in PlayerPrefs");

        audioMixer.SetFloat(MUSIC_VOLUME, Mathf.Log10(volume) * 40);
        PlayerPrefs.SetFloat(MUSIC_VOLUME, volume);
        
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        Debug.Log("VolumeSettings: Set SFX Volume: " + volume+ " in PlayerPrefs");

        audioMixer.SetFloat(SFX_VOLUME, Mathf.Log10(volume) * 40);
        PlayerPrefs.SetFloat(SFX_VOLUME, volume);
    }

    private void LoadMusicVolume()
    {
        float volume = PlayerPrefs.GetFloat(MUSIC_VOLUME);
        musicSlider.value = volume;
        Debug.Log("VolumeSettings: Load Music Volume:" + volume + " from PlayerPrefs");
        SetMusicVolume();
    }

    private void LoadSFXVolume()
    {
        float volume = PlayerPrefs.GetFloat(SFX_VOLUME);
        sfxSlider.value = volume;
        Debug.Log("VolumeSettings: Load SFX Volume: " + volume + " from PlayerPrefs");
        SetSFXVolume();
    }
}
