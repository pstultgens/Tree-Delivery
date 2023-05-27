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
    [SerializeField] public bool showUIPackages;

    [SerializeField] public bool randomizeOrderUIPackages;
    [SerializeField] public bool randomizePackageValues;
    [SerializeField] public bool canPackageBeDeliveredAtWrongNode;
    [SerializeField] public bool spawnPackageAfterPackage;

    [SerializeField] public int acceptableNumberOfWrongDeliveries = 3;


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

    public void SetLevelDifficulty(LevelEnum levelEnum)
    {
        currentLevel = levelEnum;
        UpdateHintStats();
    }


    public LevelEnum DetermineNextLevel()
    {
        Debug.Log("Current Level: " + currentLevel.ToString());
        LevelEnum nextLevel;
        switch (currentLevel)
        {
            case LevelEnum.Tutorial:
                nextLevel = LevelEnum.Test;
                break;
            case LevelEnum.Test:
                if (ScoreController.countWrongDeliveries >= acceptableNumberOfWrongDeliveries)
                {
                    nextLevel = LevelEnum.Easy1;
                }
                else
                {
                    if (!PlayerPrefsRepository.Instance.AllEasyLevelsUnlocked())
                    {
                        PlayerPrefsRepository.Instance.UnlockAllEasyLevels();
                    }                    
                    nextLevel = LevelEnum.Hard1;
                }
                break;
            case LevelEnum.Easy1:
                nextLevel = LevelEnum.Easy2;
                break;
            case LevelEnum.Easy2:
                nextLevel = LevelEnum.Easy3;
                break;
            case LevelEnum.Easy3:
                nextLevel = LevelEnum.Easy4;
                break;
            case LevelEnum.Easy4:
                nextLevel = LevelEnum.Easy5;
                break;
            case LevelEnum.Easy5:
                nextLevel = LevelEnum.Easy6;
                break;
            case LevelEnum.Easy6:
                nextLevel = LevelEnum.Easy7;
                break;
            case LevelEnum.Easy7:
                nextLevel = LevelEnum.Hard1;
                break;
            case LevelEnum.Hard1:
                nextLevel = LevelEnum.Hard2;
                break;
            case LevelEnum.Hard2:
                nextLevel = LevelEnum.Hard3;
                break;
            case LevelEnum.Hard3:
                nextLevel = LevelEnum.Hard4;
                break;
            case LevelEnum.Hard4:
                nextLevel = LevelEnum.Hard5;
                break;
            case LevelEnum.Hard5:
                nextLevel = LevelEnum.Hard6;
                break;
            case LevelEnum.Hard6:
                nextLevel = LevelEnum.Hard7;
                break;
            default:
                Debug.LogWarning("Unable to determine difficulty, back to main menu!");
                nextLevel = LevelEnum.MainMenu;
                break;
        }

        return nextLevel;
    }

    private void UpdateHintStats()
    {
        Debug.Log("Update Hint Stats for: " + currentLevel.GetName());

        switch (currentLevel)
        {
            case LevelEnum.MainMenu:
                break;
            case LevelEnum.Tutorial:
                Debug.Log("Set Tutorial Mode stats");
                SetHints(true, true, true, true, true, true, true);
                SetDifficulties(false, false, false, false);
                break;
            case LevelEnum.Test:
                Debug.Log("Set Test Mode stats");
                SetHints(false, false, false, false, false, false, true);
                SetDifficulties(true, true, true, false);
                acceptableNumberOfWrongDeliveries = 3;
                break;
            case LevelEnum.Easy1:
                Debug.Log("Set Easy 1 Mode stats");
                SetHints(true, true, true, true, true, true, true);
                SetDifficulties(false, false, false, false);
                break;
            case LevelEnum.Easy2:
                Debug.Log("Set Easy 2 Mode stats");
                SetHints(true, true, true, true, true, true, true);
                SetDifficulties(false, false, false, false);
                break;
            case LevelEnum.Easy3:
                Debug.Log("Set Easy 3 Mode stats");
                SetHints(true, true, true, true, true, true, true);
                SetDifficulties(false, false, false, false);
                break;
            case LevelEnum.Easy4:
                Debug.Log("Set Easy 4 Mode stats");
                SetHints(false, true, true, true, true, true, true);
                SetDifficulties(false, false, false, false);
                break;
            case LevelEnum.Easy5:
                Debug.Log("Set Easy 5 Mode stats");
                SetHints(false, true, true, true, true, true, true);
                SetDifficulties(false, false, false, false);
                break;
            case LevelEnum.Easy6:
                Debug.Log("Set Easy 6 Mode stats");
                SetHints(false, true, true, true, true, true, true);
                SetDifficulties(false, false, false, false);
                break;
            case LevelEnum.Easy7:
                Debug.Log("Set Easy 7 Mode stats");
                SetHints(false, false, true, true, true, true, true);
                SetDifficulties(false, true, false, false);
                break;
            case LevelEnum.Hard1:
                Debug.Log("Set Hard 1 Mode stats");
                SetHints(false, false, true, true, true, false, true);
                SetDifficulties(true, true, false, false);
                break;
            case LevelEnum.Hard2:
                Debug.Log("Set Hard 2 Mode stats");
                SetHints(false, false, true, true, true, false, true);
                SetDifficulties(true, true, false, false);
                break;
            case LevelEnum.Hard3:
                Debug.Log("Set Hard 3 Mode stats");
                SetHints(false, false, true, true, true, false, true);
                SetDifficulties(true, true, false, false);
                break;
            case LevelEnum.Hard4:
                Debug.Log("Set Hard 4 Mode stats");
                SetHints(false, false, false, false, true, false, true);
                SetDifficulties(true, true, true, false);
                break;
            case LevelEnum.Hard5:
                Debug.Log("Set Hard 5 Mode stats");
                SetHints(false, false, false, false, true, false, true);
                SetDifficulties(true, true, true, false);
                break;
            case LevelEnum.Hard6:
                Debug.Log("Set Hard 6 Mode stats");
                SetHints(false, false, false, false, false, false, true);
                SetDifficulties(true, true, true, true);
                break;
            case LevelEnum.Hard7:
                Debug.Log("Set Hard 7 Mode stats");
                SetHints(false, false, false, false, false, false, false);
                SetDifficulties(true, true, true, true);
                break;
            default:
                Debug.LogWarning("Undefined Level Mode in DifficultyController!");
                break;
        }
    }

    private void SetHints(
        bool showAlreadyCorrectValueOnNode, bool showHintValueWhenWrongDelivered, bool showHintColorWhenDelivered,
        bool showHintUIPackageAndMinimapNode, bool showPackageOnMinimap, bool showPackageValueOnMinimap,
        bool showUIPackages)
    {
        this.showAlreadyCorrectValueOnNode = showAlreadyCorrectValueOnNode;
        this.showHintValueWhenWrongDelivered = showHintValueWhenWrongDelivered;
        this.showHintColorWhenDelivered = showHintColorWhenDelivered;
        this.showHintUIPackageAndMinimapNode = showHintUIPackageAndMinimapNode;
        this.showPackageOnMinimap = showPackageOnMinimap;
        this.showPackageValueOnMinimap = showPackageValueOnMinimap;
        this.showUIPackages = showUIPackages;
    }

    private void SetDifficulties(bool randomizeOrderUIPackages, bool randomizePackageValues, bool canPackageBeDeliveredAtWrongNode, bool spawnPackageAfterPackage)
    {
        this.randomizeOrderUIPackages = randomizeOrderUIPackages;
        this.randomizePackageValues = randomizePackageValues;
        this.canPackageBeDeliveredAtWrongNode = canPackageBeDeliveredAtWrongNode;
        this.spawnPackageAfterPackage = spawnPackageAfterPackage;
    }
}
