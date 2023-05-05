using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using Cinemachine;
using UnityEngine.EventSystems;
using TMPro;


public class SceneManager : MonoBehaviour
{
    public static string selectedLevel;
    public static string selectedCar;

    [SerializeField] public Animator fadeTransition;
    [SerializeField] public float tranistionTime = 1f;

    [Header("Instantiate Player")]
    [SerializeField] public GameObject playerYellowPickupPrefab;
    [SerializeField] public GameObject playerRedPickupPrefab;
    [SerializeField] public GameObject playerBluePickupPrefab;
    [SerializeField] public Transform spawnPlayerPosition;

    [Header("Pause Menu")]
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public GameObject pauseMenuFirstSelectedButton;

    [Header("Level Complete")]
    [SerializeField] public GameObject levelCompleteWindow;
    [SerializeField] public GameObject levelCompleteFirstSelectedButton;
    [SerializeField] public TextMeshProUGUI levelCompleteScoreText;

    [Header("Audio Mixers")]
    [SerializeField] public AudioMixer audioMixer;

    private bool isLoadingLevel;
    private bool isShowingLevelComplete;

    private PlayerInputActions playerActions;
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Awake()
    {
        if (selectedCar != null)
        {
            InstantiatePlayer();
        }

        MusicController.Instance.Unmute();
    }

    private void OnEnable()
    {
        playerActions = new PlayerInputActions();
        playerActions.Enable();

        playerActions.Player.Pause.performed += PauseGame;
    }

    private void OnDisable()
    {
        playerActions.Enable();
    }

    // Instantiate player with selected car stats
    private void InstantiatePlayer()
    {
        GameObject selectedCarPrefab = null;
        CarController carController;

        switch (selectedCar)
        {
            case "CarYellow":
                selectedCarPrefab = playerYellowPickupPrefab;
                carController = playerYellowPickupPrefab.GetComponent<CarController>();
                carController.maxSpeed = 15f;
                carController.accelerationFactor = 20f;
                carController.turnFactor = 3.5f;
                carController.boostSpeed = carController.maxSpeed + 7.5f;

                break;
            case "CarRed":
                selectedCarPrefab = playerRedPickupPrefab;
                carController = playerRedPickupPrefab.GetComponent<CarController>();
                carController.maxSpeed = 20f;
                carController.accelerationFactor = 12.5f;
                carController.turnFactor = 5f;
                carController.boostSpeed = carController.maxSpeed + 7.5f;

                break;
            case "CarBlue":
                selectedCarPrefab = playerBluePickupPrefab;
                carController = playerBluePickupPrefab.GetComponent<CarController>();
                carController.maxSpeed = 10f;
                carController.accelerationFactor = 27.5f;
                carController.turnFactor = 2f;
                carController.boostSpeed = carController.maxSpeed + 7.5f;

                break;
        }

        GameObject player = Instantiate(selectedCarPrefab, new Vector2(spawnPlayerPosition.position.x, spawnPlayerPosition.position.y), Quaternion.identity);

        // Set player as thing to follow for the MainCamera and the MinimapCamera
        //Camera.main.GetComponent<FollowCamera>().thingToFollow = player;
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = player.transform;
        cinemachineVirtualCamera.LookAt = player.transform;

        GameObject minimapCamera = GameObject.FindGameObjectWithTag("MinimapCamera");
        minimapCamera.GetComponent<FollowCamera>().thingToFollow = player;
    }

    private void PauseGame(InputAction.CallbackContext context)
    {
        if (pauseMenu == null)
        {
            Debug.Log("Pause Menu in LevelLoader not set");
            return;
        }

        if (isShowingLevelComplete)
        {
            return;
        }

        // Set UI first selected button
        EventSystem eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(pauseMenuFirstSelectedButton, new BaseEventData(eventSystem));

        // Mute audio when paused
        MusicController.Instance.Mute();

        pauseMenu.SetActive(true);

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        // Resume audio when resuming
        MusicController.Instance.Unmute();

        pauseMenu.SetActive(false);

        Time.timeScale = 1f;
    }

    public void BackToMainMenu()
    {
        MusicController.Instance.Unmute();
        MusicController.Instance.LoadVolumeSettings();
        selectedCar = null;
        selectedLevel = null;
        Time.timeScale = 1f;
        GoToScene("Main Menu");
    }

    public void ShowLevelComplete()
    {
        if (isShowingLevelComplete)
        {
            return;
        }
        isShowingLevelComplete = true;

        // Set UI first selected button
        EventSystem eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(levelCompleteFirstSelectedButton, new BaseEventData(eventSystem));

        // Show Level Complete
        if (levelCompleteWindow == null)
        {
            Debug.Log("Level Complete Window in LevelLoader not set");
            return;
        }

        StartCoroutine(ShowLevelCompleteCoroutine());
    }

    IEnumerator ShowLevelCompleteCoroutine()
    {
        Debug.Log("Start Fade");
        fadeTransition.SetTrigger("Start");
                    
        yield return new WaitForSeconds(tranistionTime);

        levelCompleteWindow.SetActive(true);
        Debug.Log("End Fade");
        fadeTransition.SetTrigger("End");

        Debug.Log("Done");
        Time.timeScale = 0f;
        audioMixer.SetFloat("SFXVolume", -80f);
    }

    public void LoadNextLevel()
    {
        if (isLoadingLevel)
        {
            return;
        }
        Time.timeScale = 1f;
        isLoadingLevel = true;
        Debug.Log("Load next Level...");

        switch (DifficultyController.Instance.DetermineDifficulty())
        {
            case LevelDifficultyEnum.Test:
                StartCoroutine(LoadLevelCoroutine("Test Difficulty Level"));
                break;
            case LevelDifficultyEnum.Easy1:
                StartCoroutine(LoadLevelCoroutine("Easy Level 1"));
                break;
            case LevelDifficultyEnum.Easy2:
                StartCoroutine(LoadLevelCoroutine("Easy Level 2"));
                break;
            case LevelDifficultyEnum.Easy3:
                StartCoroutine(LoadLevelCoroutine("Easy Level 3"));
                break;
            case LevelDifficultyEnum.Hard1:
                StartCoroutine(LoadLevelCoroutine("Hard Level 1"));
                break;
            case LevelDifficultyEnum.Hard2:
                StartCoroutine(LoadLevelCoroutine("Hard Level 2"));
                break;
            default:
                Debug.LogWarning("Unable to load level by difficulty!");
                BackToMainMenu();
                break;
        }
    }

    public void LoadLevel(int levelIndex)
    {
        StartCoroutine(LoadLevelCoroutine(levelIndex));
    }

    public void GoToScene(string sceneName)
    {
        StartCoroutine(LoadLevelCoroutine(sceneName));
    }

    public void CarSelect(string carName)
    {
        selectedCar = carName;
        
        DifficultyController.Instance.currentLevelDifficulty = LevelDifficultyEnum.Tutorial;

        StartCoroutine(LoadLevelCoroutine("Tutorial"));
    }

    public void LevelSelect(string sceneName)
    {
        selectedLevel = sceneName;
        StartCoroutine(LoadLevelCoroutine(sceneName));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadLevelCoroutine(int levelIndex)
    {
        // Play animation
        fadeTransition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(tranistionTime);

        // Load Scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelIndex);
    }

    IEnumerator LoadLevelCoroutine(string sceneName)
    {
        // Play animation
        fadeTransition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(tranistionTime);

        // Load Scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
