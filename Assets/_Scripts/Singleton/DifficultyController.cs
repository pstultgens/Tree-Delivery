using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    public static DifficultyController Instance { get; private set; }

    [SerializeField] public LevelEnum currentLevelDifficulty = LevelEnum.Tutorial;
    [SerializeField] public bool showAlreadyCorrectValueOnNode;
    [SerializeField] public bool showHintValueWhenWrongDelivered;
    [SerializeField] public bool showHintColorWhenDelivered; // Correct or wrong
    [SerializeField] public bool showHintUIPackageAndMinimap;
    [SerializeField] public bool showPackageOnMinimap;
    [SerializeField] public bool showPackageValueOnMinimap;
    [SerializeField] public bool canPackageBeDeliveredAtWrongNode;
    [SerializeField] public bool showUIPackages;
    [SerializeField] public bool randomizeOrderUIPackages;
    [SerializeField] public bool randomizePackageValues;

    [SerializeField] public int acceptableNumberOfWrongDeliveries = 3;

    private int countWrongDeliveries = 0;

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
        UpdateDifficultyStats();
    }

    public void IncreaseWrongDelivery()
    {
        countWrongDeliveries++;
    }

    public LevelEnum DetermineNextLevel()
    {
        Debug.Log("Current Level Difficulty: " + currentLevelDifficulty.ToString());
        switch (currentLevelDifficulty)
        {
            case LevelEnum.Tutorial:
                currentLevelDifficulty = LevelEnum.Test;
                break;
            case LevelEnum.Test:
                if (countWrongDeliveries > acceptableNumberOfWrongDeliveries)
                {
                    currentLevelDifficulty = LevelEnum.Easy1;
                }
                else
                {
                    currentLevelDifficulty = LevelEnum.Hard1;
                }
                break;
            case LevelEnum.Easy1:
                currentLevelDifficulty = LevelEnum.Easy2;
                break;
            case LevelEnum.Easy2:
                currentLevelDifficulty = LevelEnum.Easy3;
                break;
            case LevelEnum.Hard1:
                currentLevelDifficulty = LevelEnum.Hard2;
                break;
            default:
                Debug.LogWarning("Unable to determine difficulty, back to main menu!");
                currentLevelDifficulty = LevelEnum.MainMenu;
                break;
        }
        Debug.Log("Next Determined Level Difficulty: " + currentLevelDifficulty.ToString());

        UpdateDifficultyStats();

        return currentLevelDifficulty;
    }

    private void UpdateDifficultyStats()
    {
        Debug.Log("Update Difficulty Stats");

        switch (currentLevelDifficulty)
        {
            case LevelEnum.MainMenu:
                break;
            case LevelEnum.Tutorial:
                Debug.Log("Set Tutorial Mode stats");
                showAlreadyCorrectValueOnNode = true;
                showHintValueWhenWrongDelivered = false;
                showHintColorWhenDelivered = true;
                showHintUIPackageAndMinimap = true;
                showPackageOnMinimap = true;
                showPackageValueOnMinimap = true;
                canPackageBeDeliveredAtWrongNode = false;
                showUIPackages = true;
                randomizePackageValues = true;
                randomizeOrderUIPackages = true;
                break;
            case LevelEnum.Test:
                Debug.Log("Set Test Mode stats");
                showAlreadyCorrectValueOnNode = false;
                showHintValueWhenWrongDelivered = false;
                showHintColorWhenDelivered = true;
                showHintUIPackageAndMinimap = true;
                showPackageOnMinimap = true;
                showPackageValueOnMinimap = true;
                canPackageBeDeliveredAtWrongNode = false;
                showUIPackages = true;
                randomizePackageValues = true;
                randomizeOrderUIPackages = true;
                break;
            case LevelEnum.Easy1:
                Debug.Log("Set Easy 1 Mode stats");
                showAlreadyCorrectValueOnNode = true;
                showHintValueWhenWrongDelivered = true;
                showHintColorWhenDelivered = true;
                showHintUIPackageAndMinimap = true;
                showPackageOnMinimap = true;
                showPackageValueOnMinimap = true;
                canPackageBeDeliveredAtWrongNode = false;
                showUIPackages = true;
                randomizePackageValues = false;
                randomizeOrderUIPackages = false;
                break;
            case LevelEnum.Easy2:
                Debug.Log("Set Easy 2 Mode stats");
                showAlreadyCorrectValueOnNode = false;
                showHintValueWhenWrongDelivered = true;
                showHintColorWhenDelivered = true;
                showHintUIPackageAndMinimap = true;
                showPackageOnMinimap = true;
                showPackageValueOnMinimap = true;
                canPackageBeDeliveredAtWrongNode = false;
                showUIPackages = true;
                randomizePackageValues = true;
                randomizeOrderUIPackages = false;
                break;
            case LevelEnum.Easy3:
                Debug.Log("Set Easy 3 Mode stats");
                showAlreadyCorrectValueOnNode = false;
                showHintValueWhenWrongDelivered = true;
                showHintColorWhenDelivered = true;
                showHintUIPackageAndMinimap = true;
                showPackageOnMinimap = true;
                showPackageValueOnMinimap = true;
                canPackageBeDeliveredAtWrongNode = false;
                showUIPackages = true;
                randomizePackageValues = true;
                randomizeOrderUIPackages = false;
                break;
            case LevelEnum.Hard1:
                Debug.Log("Set Hard 1 Mode stats");
                showAlreadyCorrectValueOnNode = false;
                showHintValueWhenWrongDelivered = false;
                showHintColorWhenDelivered = true;
                showHintUIPackageAndMinimap = true;
                showPackageOnMinimap = true;
                showPackageValueOnMinimap = true;
                canPackageBeDeliveredAtWrongNode = false;
                showUIPackages = true;
                randomizePackageValues = true;
                randomizeOrderUIPackages = true;
                break;
            case LevelEnum.Hard2:
                Debug.Log("Set Hard 2 Mode stats");
                showAlreadyCorrectValueOnNode = false;
                showHintValueWhenWrongDelivered = false;
                showHintColorWhenDelivered = true;
                showHintUIPackageAndMinimap = true;
                showPackageOnMinimap = false;
                showPackageValueOnMinimap = false;
                canPackageBeDeliveredAtWrongNode = false;
                showUIPackages = false;
                randomizePackageValues = true;
                randomizeOrderUIPackages = true;
                break;
            default:
                Debug.LogWarning("Undefined Level Mode in DifficultyController!");
                break;
        }
    }
}
