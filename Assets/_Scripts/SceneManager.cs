using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using Cinemachine;
using UnityEngine.EventSystems;
using TMPro;


public class SceneManager : MonoBehaviour
{
    public static bool isGamePaused;
    public static string selectedLevel;
    public static string selectedCar;

    [SerializeField] public Animator fadeTransition;
    [SerializeField] public float tranistionTime = 1f;

    [Header("Instantiate Player")]
    [SerializeField] public GameObject playerYellowPickupPrefab;
    [SerializeField] public GameObject playerRedPickupPrefab;
    [SerializeField] public GameObject playerBluePickupPrefab;
    [SerializeField] public Transform spawnPlayerPosition;

    [Header("Pause Window")]
    [SerializeField] public GameObject pauseWindow;
    [SerializeField] public GameObject pauseWindowFirstSelectedButton;

    [Header("Settings Window")]
    [SerializeField] public GameObject settingsWindow;
    [SerializeField] public GameObject settingsWindowFirstSelectedButton;

    [Header("Level Complete")]
    [SerializeField] public GameObject levelCompleteWindow;
    [SerializeField] public GameObject levelCompleteFirstSelectedButton;

    [Header("Input Highscore Window")]
    [SerializeField] public GameObject inputHighscoreWindow;
    [SerializeField] public GameObject inputHighscoreFirstSelectedButton;

    [Header("Highscore Table Window")]
    [SerializeField] public GameObject highscoreTableWindow;
    [SerializeField] public GameObject highscoreTableFirstSelectedButton;

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
    }

    private void OnEnable()
    {
        playerActions = new PlayerInputActions();
        playerActions.Enable();

        playerActions.Player.Pause.performed += PauseGameCallBackContext;
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
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = player.transform;
        cinemachineVirtualCamera.LookAt = player.transform;

        GameObject minimapCamera = GameObject.FindGameObjectWithTag("MinimapCamera");
        minimapCamera.GetComponent<FollowCamera>().thingToFollow = player;
    }

    public void ResumeGame()
    {
        StartCoroutine(ResumeGameCoroutine());
    }

    private IEnumerator ResumeGameCoroutine()
    {
        fadeTransition.SetTrigger("Start");
        yield return new WaitForSeconds(tranistionTime);
        pauseWindow.SetActive(false);
        isGamePaused = false;
        fadeTransition.SetTrigger("End");
    }

    private void PauseGameCallBackContext(InputAction.CallbackContext context)
    {
        if (isGamePaused)
        {
            return;
        }
        ShowPauseWindow();
    }

    public void ShowPauseWindow()
    {
        if (pauseWindow == null)
        {
            Debug.Log("Pause Window in SceneManager not set");
            return;
        }

        if (isShowingLevelComplete)
        {
            return;
        }

        isGamePaused = true;

        StartCoroutine(ShowPauseWindowCoroutine());
    }

    private IEnumerator ShowPauseWindowCoroutine()
    {
        fadeTransition.SetTrigger("Start");
        yield return new WaitForSeconds(tranistionTime);
        settingsWindow.SetActive(false);
        pauseWindow.SetActive(true);
        SetFirstSelectedUIButton(pauseWindowFirstSelectedButton);
        fadeTransition.SetTrigger("End");
    }

    public void ShowSettingsWindow()
    {
        // This shows the window from the pause menu, not the Settings Menu scene
        if (settingsWindow == null)
        {
            Debug.Log("Settings Window in SceneManager not set");
            return;
        }

        StartCoroutine(ShowSettingsWindowCoroutine());
    }

    private IEnumerator ShowSettingsWindowCoroutine()
    {
        fadeTransition.SetTrigger("Start");
        yield return new WaitForSeconds(tranistionTime);
        pauseWindow.SetActive(false);
        settingsWindow.SetActive(true);
        SetFirstSelectedUIButton(settingsWindowFirstSelectedButton);
        fadeTransition.SetTrigger("End");
    }

    public void BackToMainMenu()
    {
        MusicController.Instance.LoadVolumeSettings();
        isGamePaused = false;
        selectedCar = null;
        selectedLevel = null;
        GoToScene("Main Menu");
    }

    public void ShowLevelComplete()
    {
        if (isShowingLevelComplete)
        {
            return;
        }
        isShowingLevelComplete = true;
        isGamePaused = true;

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
        fadeTransition.SetTrigger("Start");
        yield return new WaitForSeconds(tranistionTime);
        levelCompleteWindow.SetActive(true);
        SetFirstSelectedUIButton(levelCompleteFirstSelectedButton);
        fadeTransition.SetTrigger("End");
    }

    public void ContinueAfterLevelCompleted()
    {
        HighscoreTable highscoreTable = highscoreTableWindow.GetComponent<HighscoreTable>();
        if (highscoreTable.IsHighscoreInTop10(DifficultyController.Instance.currentLevelDifficulty, ScoreController.currentScore))
        {
            // Input Highscore Window
            StartCoroutine(ShowInputHighscoreCoroutine());
        }
        else
        {
            // Show Highscore Table Window
            StartCoroutine(ShowHighscoreTableCoroutine());
        }
    }

    public void InputHighscore()
    {
        InputHighscore input = inputHighscoreWindow.GetComponent<InputHighscore>();

        if (string.IsNullOrEmpty(input.GetName()))
        {
            return;
        }

        HighscoreTable highscoreTable = highscoreTableWindow.GetComponent<HighscoreTable>();
        highscoreTable.AddHighscoreEntry(DifficultyController.Instance.currentLevelDifficulty, ScoreController.currentScore, input.GetName());

        StartCoroutine(ShowHighscoreTableCoroutine());
    }

    IEnumerator ShowInputHighscoreCoroutine()
    {
        fadeTransition.SetTrigger("Start");
        yield return new WaitForSeconds(tranistionTime);
        levelCompleteWindow.SetActive(false);
        inputHighscoreWindow.SetActive(true);
        SetFirstSelectedUIButton(inputHighscoreFirstSelectedButton);
        fadeTransition.SetTrigger("End");
    }

    IEnumerator ShowHighscoreTableCoroutine()
    {
        fadeTransition.SetTrigger("Start");
        yield return new WaitForSeconds(tranistionTime);
        levelCompleteWindow.SetActive(false);
        inputHighscoreWindow.SetActive(false);
        highscoreTableWindow.SetActive(true);

        HighscoreTable highscoreTable = highscoreTableWindow.GetComponent<HighscoreTable>();
        highscoreTable.ShowHighscores(DifficultyController.Instance.currentLevelDifficulty);

        SetFirstSelectedUIButton(highscoreTableFirstSelectedButton);
        fadeTransition.SetTrigger("End");
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

        switch (DifficultyController.Instance.DetermineNextLevel())
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

    private void SetFirstSelectedUIButton(GameObject firstSelectedButton)
    {
        EventSystem eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(firstSelectedButton, new BaseEventData(eventSystem));
        SimulateOnSelect(firstSelectedButton);
    }

    private void SimulateOnSelect(GameObject selectedGameObject)
    {
        // Create a new pointer event
        PointerEventData pointerEvent = new PointerEventData(EventSystem.current);

        // Simulate an OnSelect event
        ExecuteEvents.Execute(selectedGameObject, pointerEvent, ExecuteEvents.selectHandler);
    }
}