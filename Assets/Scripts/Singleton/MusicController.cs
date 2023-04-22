using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicController : Singleton
{
    [Header("Audio Mixers")]
    [SerializeField] public AudioMixer audioMixer;

    [Header("Audio sources")]
    [SerializeField] public AudioSource menuAudioSource;
    [SerializeField] public AudioSource levelAudioSource;
    [SerializeField] public AudioSource menuNavigationAudioSource;
    [SerializeField] public AudioSource buttonSubmitAudioSource;

    void Start()
    {
        audioMixer.SetFloat("MusicVolume", -6f);
        audioMixer.SetFloat("UIVolume", 0f);
    }

    void Update()
    {
        if (!menuAudioSource.isPlaying)
        {
            menuAudioSource.Play();
        }

        if (SceneManager.GetActiveScene().name.Equals("Main Menu") ||
            SceneManager.GetActiveScene().name.Equals("Select Car Menu"))
        {
            levelAudioSource.Stop();
            if (!menuAudioSource.isPlaying)
            {
                menuAudioSource.Play();
            }
        }
        else
        {
            menuAudioSource.Stop();
            if (!levelAudioSource.isPlaying)
            {
                levelAudioSource.Play();
            }
        }
    }

    public void PlayMenuNavigationSFX()
    {
        menuNavigationAudioSource.Play();

    }

    public void PlayButtonSubmitSFX()
    {
        buttonSubmitAudioSource.Play();
    }
}
