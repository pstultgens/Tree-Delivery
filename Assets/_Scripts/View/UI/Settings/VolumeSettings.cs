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
        LoadMusicVolume();
        LoadSFXVolume();
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat(MUSIC_VOLUME, Mathf.Log10(volume) * 40);
        PlayerPrefsRepository.Instance.SetMusicVolume(volume);

    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat(SFX_VOLUME, Mathf.Log10(volume) * 40);
        PlayerPrefsRepository.Instance.SetSFXVolume(volume);
    }

    private void LoadMusicVolume()
    {
        float volume = PlayerPrefsRepository.Instance.LoadMusicVolume();
        musicSlider.value = volume;
        SetMusicVolume();
    }

    private void LoadSFXVolume()
    {
        float volume = PlayerPrefsRepository.Instance.LoadSFXVolume();
        sfxSlider.value = volume;
        SetSFXVolume();
    }
}
