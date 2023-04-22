using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicController : Singleton
{
    [Header("Audio Mixers")]
    [SerializeField] public AudioMixer audioMixer;

    [Header("Game Music Audio sources")]   
    [SerializeField] public AudioSource levelAudioSource;

    [Header("Menu Audio sources")]
    [SerializeField] public AudioSource menuAudioSource;
    [SerializeField] public AudioSource menuNavigationAudioSource;
    [SerializeField] public AudioSource buttonSubmitAudioSource;

    [Header("Package SFX Audio sources")]
    [SerializeField] public AudioSource pickupAudioSource;
    [SerializeField] public AudioSource dropAudioSource;
    [SerializeField] public AudioSource correctDeliveredAudioSource;
    [SerializeField] public AudioSource wrongDeliveredAudioSource;


    void Start()
    {
        audioMixer.SetFloat("MusicVolume", -10f);
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

    public void PlayPickupSFX()
    {
        pickupAudioSource.Play();
    }

    public void PlayDropSFX()
    {
        dropAudioSource.Play();
    }

    public void PlayCorrectDeliveredSFX()
    {
        correctDeliveredAudioSource.Play();
    }

    public void PlayWrongDeliveredSFX()
    {
        wrongDeliveredAudioSource.Play();
    }
}
