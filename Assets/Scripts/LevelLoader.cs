using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Audio;

public class LevelLoader : MonoBehaviour
{
    public static string selectedLevel;
    public static string selectedCar;

    [SerializeField] public Animator transition;
    [SerializeField] public float tranistionTime = 1f;

    [Header("Instantiate Player")]
    [SerializeField] public GameObject playerPrefab;
    [SerializeField] public Transform spawnPlayerPosition;
    [SerializeField] public Sprite carYellowSprite;
    [SerializeField] public Sprite carRedSprite;
    [SerializeField] public Sprite carBlueSprite;

    [Header("Pause Menu")]
    [SerializeField] public GameObject pauseMenu;

    [Header("Audio Mixers")]
    [SerializeField] public AudioMixer audioMixer;

    private bool isLoadingLevel;
    private PlayerInputActions playerActions;
    private DifficultyController difficultyController;

    private void Awake()
    {
        playerActions = new PlayerInputActions();
        difficultyController = FindObjectOfType<DifficultyController>();

        if (selectedCar != null)
        {
            InstantiatePlayer();
        }
    }

    private void OnEnable()
    {
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
        CarController carController = playerPrefab.GetComponent<CarController>();
        SpriteRenderer[] carSriteRenderers = playerPrefab.GetComponentsInChildren<SpriteRenderer>();

        switch (selectedCar)
        {
            case "CarYellow":
                carController.maxSpeed = 15f;
                carController.accelerationFactor = 20f;
                carController.turnFactor = 3.5f;
                carController.boostSpeed = carController.maxSpeed + 7.5f;
                SetCarSprites(carSriteRenderers, carYellowSprite);
                break;
            case "CarRed":
                carController.maxSpeed = 20f;
                carController.accelerationFactor = 12.5f;
                carController.turnFactor = 5f;
                carController.boostSpeed = carController.maxSpeed + 7.5f;
                SetCarSprites(carSriteRenderers, carRedSprite);
                break;
            case "CarBlue":
                carController.maxSpeed = 10f;
                carController.accelerationFactor = 27.5f;
                carController.turnFactor = 2f;
                carController.boostSpeed = carController.maxSpeed + 7.5f;
                SetCarSprites(carSriteRenderers, carBlueSprite);
                break;
        }

        GameObject player = Instantiate(playerPrefab, new Vector2(spawnPlayerPosition.position.x, spawnPlayerPosition.position.y), Quaternion.identity);

        // Set player as thing to follow for the MainCamera and the MinimapCamera
        Camera.main.GetComponent<FollowCamera>().thingToFollow = player;
        GameObject minimapCamera = GameObject.FindGameObjectWithTag("MinimapCamera");
        minimapCamera.GetComponent<FollowCamera>().thingToFollow = player;
    }

    // Sets the sprite of the car and minimapIcon
    private void SetCarSprites(SpriteRenderer[] carSriteRenderers, Sprite sprite)
    {
        foreach (SpriteRenderer spriteRenderer in carSriteRenderers)
        {
            spriteRenderer.sprite = sprite;
        }
    }

    private void PauseGame(InputAction.CallbackContext context)
    {
        if (pauseMenu == null)
        {
            Debug.Log("Pause Menu in LevelLoader not set");
            return;
        }

        // Mute audio when paused
        audioMixer.SetFloat("SFXVolume", -80f);
        audioMixer.SetFloat("MusicVolume", -80f);

        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        // Resume audio when resuming
        audioMixer.SetFloat("SFXVolume", 0f);
        audioMixer.SetFloat("MusicVolume", -6f);

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BackToMainMenu()
    {
        selectedCar = null;
        selectedLevel = null;
        Time.timeScale = 1f;
        GoToScene("Main Menu");
    }

    public void LoadNextLevel()
    {
        if (isLoadingLevel)
        {
            return;
        }

        isLoadingLevel = true;
        Debug.Log("Load next Level...");

        switch (difficultyController.DetermineDifficulty())
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

        // When Select Level Menu is implemented
        //StartCoroutine(LoadLevelCoroutine("Select Level Menu"));
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
        transition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(tranistionTime);

        // Load Scene
        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator LoadLevelCoroutine(string sceneName)
    {
        // Play animation
        transition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(tranistionTime);

        // Load Scene
        SceneManager.LoadScene(sceneName);
    }
}
