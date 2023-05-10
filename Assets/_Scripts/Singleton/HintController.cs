using UnityEngine;

public class HintController : MonoBehaviour
{
    public static HintController Instance { get; private set; }

    [SerializeField] public LevelEnum currentLevel = LevelEnum.Tutorial;

    [Header("READ ONLY STATS")]
    [SerializeField] public bool showAlreadyCorrectValueOnNode;
    [SerializeField] public bool showHintValueWhenWrongDelivered;
    [SerializeField] public bool showHintColorWhenDelivered; // Correct or wrong
    [SerializeField] public bool showHintUIPackageAndMinimapNode;
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
        UpdateHintStats();
    }

    public void IncreaseWrongDelivery()
    {
        countWrongDeliveries++;
    }

    public void SetLevelDifficulty(LevelEnum levelEnum)
    {
        currentLevel = levelEnum;
        UpdateHintStats();
    }


    public LevelEnum DetermineNextLevel()
    {
        Debug.Log("Current Level Difficulty: " + currentLevel.ToString());
        switch (currentLevel)
        {
            case LevelEnum.Tutorial:
                currentLevel = LevelEnum.Test;
                break;
            case LevelEnum.Test:
                if (countWrongDeliveries > acceptableNumberOfWrongDeliveries)
                {
                    currentLevel = LevelEnum.Easy1;
                }
                else
                {
                    currentLevel = LevelEnum.Hard1;
                }
                break;
            case LevelEnum.Easy1:
                currentLevel = LevelEnum.Easy2;
                break;
            case LevelEnum.Easy2:
                currentLevel = LevelEnum.Easy3;
                break;
            case LevelEnum.Hard1:
                currentLevel = LevelEnum.Hard2;
                break;
            default:
                Debug.LogWarning("Unable to determine difficulty, back to main menu!");
                currentLevel = LevelEnum.MainMenu;
                break;
        }
        Debug.Log("Next Determined Level Difficulty: " + currentLevel.ToString());

        UpdateHintStats();

        return currentLevel;
    }

    private void UpdateHintStats()
    {
        Debug.Log("Update Difficulty Stats");

        switch (currentLevel)
        {
            case LevelEnum.MainMenu:
                break;
            case LevelEnum.Tutorial:
                Debug.Log("Set Tutorial Mode stats");
                showAlreadyCorrectValueOnNode = true;
                showHintValueWhenWrongDelivered = false;
                showHintColorWhenDelivered = true;
                showHintUIPackageAndMinimapNode = true;
                showPackageOnMinimap = true;
                showPackageValueOnMinimap = true;
                canPackageBeDeliveredAtWrongNode = false;
                showUIPackages = true;
                randomizeOrderUIPackages = true;
                randomizePackageValues = true;
                break;
            case LevelEnum.Test:
                Debug.Log("Set Test Mode stats");
                showAlreadyCorrectValueOnNode = false;
                showHintValueWhenWrongDelivered = false;
                showHintColorWhenDelivered = true;
                showHintUIPackageAndMinimapNode = true;
                showPackageOnMinimap = true;
                showPackageValueOnMinimap = true;
                canPackageBeDeliveredAtWrongNode = false;
                showUIPackages = true;
                randomizeOrderUIPackages = true;
                randomizePackageValues = true;
                acceptableNumberOfWrongDeliveries = 1;
                break;
            case LevelEnum.Easy1:
                Debug.Log("Set Easy 1 Mode stats");
                showAlreadyCorrectValueOnNode = true;
                showHintValueWhenWrongDelivered = true;
                showHintColorWhenDelivered = true;
                showHintUIPackageAndMinimapNode = true;
                showPackageOnMinimap = true;
                showPackageValueOnMinimap = true;
                canPackageBeDeliveredAtWrongNode = false;
                showUIPackages = true;
                randomizeOrderUIPackages = false;
                randomizePackageValues = false;
                break;
            case LevelEnum.Easy2:
                Debug.Log("Set Easy 2 Mode stats");
                showAlreadyCorrectValueOnNode = false;
                showHintValueWhenWrongDelivered = true;
                showHintColorWhenDelivered = true;
                showHintUIPackageAndMinimapNode = true;
                showPackageOnMinimap = true;
                showPackageValueOnMinimap = true;
                canPackageBeDeliveredAtWrongNode = false;
                showUIPackages = true;
                randomizeOrderUIPackages = false;
                randomizePackageValues = true;                
                break;
            case LevelEnum.Easy3:
                Debug.Log("Set Easy 3 Mode stats");
                showAlreadyCorrectValueOnNode = false;
                showHintValueWhenWrongDelivered = true;
                showHintColorWhenDelivered = true;
                showHintUIPackageAndMinimapNode = true;
                showPackageOnMinimap = true;
                showPackageValueOnMinimap = true;
                canPackageBeDeliveredAtWrongNode = false;
                showUIPackages = true;
                randomizeOrderUIPackages = false;
                randomizePackageValues = true;                
                break;
            case LevelEnum.Hard1:
                Debug.Log("Set Hard 1 Mode stats");
                showAlreadyCorrectValueOnNode = false;
                showHintValueWhenWrongDelivered = false;
                showHintColorWhenDelivered = true;
                showHintUIPackageAndMinimapNode = true;
                showPackageOnMinimap = true;
                showPackageValueOnMinimap = true;
                canPackageBeDeliveredAtWrongNode = false;
                showUIPackages = true;
                randomizeOrderUIPackages = true;
                randomizePackageValues = true;
                break;
            case LevelEnum.Hard2:
                Debug.Log("Set Hard 2 Mode stats");
                showAlreadyCorrectValueOnNode = false;
                showHintValueWhenWrongDelivered = false;
                showHintColorWhenDelivered = true;
                showHintUIPackageAndMinimapNode = true;
                showPackageOnMinimap = false;
                showPackageValueOnMinimap = false;
                canPackageBeDeliveredAtWrongNode = false;
                showUIPackages = false;
                randomizeOrderUIPackages = true;
                randomizePackageValues = true;
                break;
            default:
                Debug.LogWarning("Undefined Level Mode in DifficultyController!");
                break;
        }
    }
}
