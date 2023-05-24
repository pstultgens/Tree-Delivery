using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using Cinemachine;
using UnityEngine.EventSystems;
using TMPro;


public class SceneManager : MonoBehaviour
{
    private static string PLAYER_PREFS_PLAYER_NAME = "PlayerName";

    public static bool isGamePaused;
    public static bool isCountingDown;

    private static LevelEnum selectedLevel;
    public static string selectedCar;

    [SerializeField] public Animator fadeTransition;
    [SerializeField] public float tranistionTime = 1f;

    [Header("Player Name")]
    [SerializeField] public TMP_InputField playerNameInputField;

    [Header("Instantiate Player")]
    [SerializeField] public GameObject playerYellowPickupPrefab;
    [SerializeField] public GameObject playerRedPickupPrefab;
    [SerializeField] public GameObject playerBluePickupPrefab;
    [SerializeField] public Transform spawnPlayerPosition;

    [Header("Pause Window")]
    [SerializeField] public GameObject pauseWindow;
    [SerializeField] public GameObject pauseWindowFirstSelectedButton;

    [Header("Controls Window")]
    [SerializeField] public GameObject controlsWindow;
    [SerializeField] public GameObject controlsWindowFirstSelectedButton;

    [Header("Settings Window")]
    [SerializeField] public GameObject settingsWindow;
    [SerializeField] public GameObject settingsWindowFirstSelectedButton;

    [Header("Level Complete")]
    [SerializeField] public GameObject levelCompleteWindow;
    [SerializeField] public GameObject levelCompleteFirstSelectedButton;

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
        controlsWindow.SetActive(false);
        pauseWindow.SetActive(true);
        SetFirstSelectedUIButton(pauseWindowFirstSelectedButton);
        fadeTransition.SetTrigger("End");
    }

    public void ShowControlsWindow()
    {
        StartCoroutine(ShowControlsWindowCoroutine());
    }

    private IEnumerator ShowControlsWindowCoroutine()
    {
        fadeTransition.SetTrigger("Start");
        yield return new WaitForSeconds(tranistionTime);
        pauseWindow.SetActive(false);
        controlsWindow.SetActive(true);
        SetFirstSelectedUIButton(controlsWindowFirstSelectedButton);
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
        isCountingDown = false;
        selectedCar = null;
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

        if (highscoreTable.IsHighscoreInTop10(HintController.Instance.currentLevel, ScoreController.currentScore))
        {
            // Input Highscore
            AddHighscore();
        }
        else
        {
            // Show Highscore Table Window
            StartCoroutine(ShowHighscoreTableCoroutine());
        }

    }

    public void AddHighscore()
    {
        string playerName = "";

        if (PlayerPrefs.HasKey(PLAYER_PREFS_PLAYER_NAME))
        {
            playerName = PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME);

        }
        else
        {
            playerName = "Debugger";
        }

        HighscoreTable highscoreTable = highscoreTableWindow.GetComponent<HighscoreTable>();
        highscoreTable.AddHighscoreEntry(HintController.Instance.currentLevel, ScoreController.currentScore, playerName);

        StartCoroutine(ShowHighscoreTableCoroutine());
    }

    IEnumerator ShowHighscoreTableCoroutine()
    {
        fadeTransition.SetTrigger("Start");
        yield return new WaitForSeconds(tranistionTime);
        levelCompleteWindow.SetActive(false);
        highscoreTableWindow.SetActive(true);

        HighscoreTable highscoreTable = highscoreTableWindow.GetComponent<HighscoreTable>();
        highscoreTable.ShowHighscores(HintController.Instance.currentLevel);

        SetFirstSelectedUIButton(highscoreTableFirstSelectedButton);
        fadeTransition.SetTrigger("End");
    }

    public void LoadNextLevel()
    {
        if (isLoadingLevel)
        {
            return;
        }
        isLoadingLevel = true;
        Debug.Log("Load next Level...");

        switch (HintController.Instance.DetermineNextLevel())
        {
            case LevelEnum.MainMenu:
                BackToMainMenu();
                break;
            case LevelEnum.Test:
                StartCoroutine(LoadLevelCoroutine("Test Difficulty Level"));
                break;
            case LevelEnum.Easy1:
                StartCoroutine(LoadLevelCoroutine("Easy Level 1"));
                break;
            case LevelEnum.Easy2:
                StartCoroutine(LoadLevelCoroutine("Easy Level 2"));
                break;
            case LevelEnum.Easy3:
                StartCoroutine(LoadLevelCoroutine("Easy Level 3"));
                break;
            case LevelEnum.Easy4:
                StartCoroutine(LoadLevelCoroutine("Easy Level 4"));
                break;
            case LevelEnum.Easy5:
                StartCoroutine(LoadLevelCoroutine("Easy Level 5"));
                break;
            case LevelEnum.Easy6:
                StartCoroutine(LoadLevelCoroutine("Easy Level 6"));
                break;
            case LevelEnum.Easy7:
                StartCoroutine(LoadLevelCoroutine("Easy Level 7"));
                break;
            case LevelEnum.Hard1:
                StartCoroutine(LoadLevelCoroutine("Hard Level 1"));
                break;
            case LevelEnum.Hard2:
                StartCoroutine(LoadLevelCoroutine("Hard Level 2"));
                break;
            case LevelEnum.Hard3:
                StartCoroutine(LoadLevelCoroutine("Hard Level 3"));
                break;
            case LevelEnum.Hard4:
                StartCoroutine(LoadLevelCoroutine("Hard Level 4"));
                break;
            case LevelEnum.Hard5:
                StartCoroutine(LoadLevelCoroutine("Hard Level 5"));
                break;
            case LevelEnum.Hard6:
                StartCoroutine(LoadLevelCoroutine("Hard Level 6"));
                break;
            case LevelEnum.Hard7:
                StartCoroutine(LoadLevelCoroutine("Hard Level 7"));
                break;
            default:
                Debug.LogWarning("Unable to load level by difficulty!");
                BackToMainMenu();
                break;
        }
    }

    public void GoToScene(string sceneName)
    {
        StartCoroutine(LoadLevelCoroutine(sceneName));
    }

    public void EnterPlayerName()
    {
        PlayerPrefs.SetString(PLAYER_PREFS_PLAYER_NAME, playerNameInputField.text);
        PlayerPrefs.Save();

        GoToScene("Select Level Menu");
    }

    public void SelectTutorialLevel() => LevelSelect(LevelEnum.Tutorial);
    public void SelectTestLevel() => LevelSelect(LevelEnum.Test);
    public void SelectLevel1() => LevelSelect(LevelEnum.Easy1);
    public void SelectLevel2() => LevelSelect(LevelEnum.Easy2);
    public void SelectLevel3() => LevelSelect(LevelEnum.Easy3);
    public void SelectLevel4() => LevelSelect(LevelEnum.Easy4);
    public void SelectLevel5() => LevelSelect(LevelEnum.Easy5);
    public void SelectLevel6() => LevelSelect(LevelEnum.Easy6);
    public void SelectLevel7() => LevelSelect(LevelEnum.Easy7);
    public void SelectLevel8() => LevelSelect(LevelEnum.Hard1);
    public void SelectLevel9() => LevelSelect(LevelEnum.Hard2);
    public void SelectLevel10() => LevelSelect(LevelEnum.Hard3);
    public void SelectLevel11() => LevelSelect(LevelEnum.Hard4);
    public void SelectLevel12() => LevelSelect(LevelEnum.Hard5);
    public void SelectLevel13() => LevelSelect(LevelEnum.Hard6);
    public void SelectLevel14() => LevelSelect(LevelEnum.Hard7);

    private void LevelSelect(LevelEnum level)
    {
        selectedLevel = level;
        GoToScene("Select Car Menu");        
    }

    public void CarSelect(string carName)
    {
        selectedCar = carName;

        HintController.Instance.SetLevelDifficulty(selectedLevel);

        StartCoroutine(LoadLevelCoroutine(selectedLevel.GetSceneName()));
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

        isGamePaused = false;
        // Load Scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelIndex);
    }

    IEnumerator LoadLevelCoroutine(string sceneName)
    {
        // Play animation
        fadeTransition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(tranistionTime);

        isGamePaused = false;
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
