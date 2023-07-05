using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance { get; private set; }

    private static string MUSIC_VOLUME = "MusicVolume";
    private static string SFX_VOLUME = "SFXVolume";

    [Header("Audio Mixers")]
    [SerializeField] public AudioMixer audioMixer;

    [Header("Game Music Audio sources")]
    [SerializeField] public AudioSource[] levelAudioSources;

    [Header("Menu Audio sources")]
    [SerializeField] public AudioSource menuAudioSource;
    [SerializeField] public AudioSource creditsAudioSource;
    [SerializeField] public AudioSource menuNavigationAudioSource;
    [SerializeField] public AudioSource buttonSubmitAudioSource;

    [Header("Package SFX Audio sources")]
    [SerializeField] public AudioSource pickupAudioSource;
    [SerializeField] public AudioSource dropAudioSource;

    private AudioSource selectedLevelAudioSource;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadVolumeSettings();
        RandomiseLevelMusic();
    }

    void Update()
    {
        if (!Application.isFocused)
        {
            return;
        }

        if (!menuAudioSource.isPlaying)
        {
            menuAudioSource.Play();
        }

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("Main Menu")
            || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("Settings Menu")
            || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("About Menu")
            || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("Enter Name Menu")
            || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("Select Level Menu")
            || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("Select Car Menu"))
        {
            selectedLevelAudioSource.Stop();
            creditsAudioSource.Stop();

            if (!menuAudioSource.isPlaying)
            {
                menuAudioSource.Play();
            }
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("Credits Menu"))
        {
            selectedLevelAudioSource.Stop();
            menuAudioSource.Stop();

            if (!creditsAudioSource.isPlaying)
            {
                creditsAudioSource.Play();
            }
        }
        else
        {
            menuAudioSource.Stop();
            creditsAudioSource.Stop();

            if (!selectedLevelAudioSource.isPlaying)
            {
                Debug.Log("Level Audio is not playing, choose random level music and play");
                RandomiseLevelMusic();
                selectedLevelAudioSource.Play();
            }
        }
    }



    private void RandomiseLevelMusic()
    {
        selectedLevelAudioSource = levelAudioSources[Random.Range(0, levelAudioSources.Length)];
    }

    public void LoadVolumeSettings()
    {
        if (PlayerPrefs.HasKey(MUSIC_VOLUME))
        {
            LoadMusicVolume();
        }
        else
        {
            DefaultMusicVolume();
        }

        if (PlayerPrefs.HasKey(SFX_VOLUME))
        {
            LoadSFXVolume();
        }
        else
        {
            DefaultSFXVolume();
        }
    }

    private void LoadMusicVolume()
    {
        float volume = PlayerPrefs.GetFloat(MUSIC_VOLUME);
        Debug.Log("MusicController: Load Music Volume: " + volume + " from PlayerPrefs");
        audioMixer.SetFloat(MUSIC_VOLUME, Mathf.Log10(volume) * 40);
    }

    private void DefaultMusicVolume()
    {
        Debug.Log("MusicController: Load Default Music Volume");
        audioMixer.SetFloat(MUSIC_VOLUME, -10f);
    }

    private void LoadSFXVolume()
    {
        float volume = PlayerPrefs.GetFloat(SFX_VOLUME);
        Debug.Log("MusicController: Load SFX Volume: " + volume + " from PlayerPrefs");
        audioMixer.SetFloat(SFX_VOLUME, Mathf.Log10(volume) * 40);
    }

    private void DefaultSFXVolume()
    {
        Debug.Log("MusicController: Load Default SFX Volume");
        audioMixer.SetFloat(SFX_VOLUME, -2f);
    }

    public void Mute()
    {
        audioMixer.SetFloat(MUSIC_VOLUME, -80f);
        audioMixer.SetFloat(SFX_VOLUME, -80f);
    }

    public void Unmute()
    {
        LoadVolumeSettings();
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
}
