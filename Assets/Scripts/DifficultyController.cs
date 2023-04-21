using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    public static LevelDifficultyEnum currentLevelDifficulty = LevelDifficultyEnum.Tutorial;
    public static bool showAlreadyCorrectValueOnNode;
    public static bool showHintValueWhenWrongDelivered;
    public static bool showHintColorWhenDelivered; // Correct or wrong
    public static bool showHintUIPackageAndMinimap;
    public static bool canPackageBeDeliveredAtWrongNode;
    public static bool showUIPackages;
    public static bool randomizeOrderUIPackages;
    public static bool randomizePackageValues;


    [SerializeField] public LevelDifficultyEnum _currentLevelDifficulty;
    [SerializeField] public int acceptableNumberOfWrongDeliveries = 3;
    private int countWrongDeliveries = 0;

    void OnValidate()
    {
        currentLevelDifficulty = _currentLevelDifficulty;
    }

    private void Awake()
    {
        Debug.Log("Current Level Difficulty: " + currentLevelDifficulty.ToString());

        switch (currentLevelDifficulty)
        {
            case LevelDifficultyEnum.Tutorial:
                Debug.Log("Set Test Mode stats");
                showAlreadyCorrectValueOnNode = true;
                showHintValueWhenWrongDelivered = false;
                showHintColorWhenDelivered = true;
                showHintUIPackageAndMinimap = true;
                canPackageBeDeliveredAtWrongNode = false;
                showUIPackages = true;
                randomizePackageValues = true;
                randomizeOrderUIPackages = true;
                break;
            case LevelDifficultyEnum.Test:
                Debug.Log("Set Test Mode stats");
                showAlreadyCorrectValueOnNode = false;
                showHintValueWhenWrongDelivered = false;
                showHintColorWhenDelivered = false;
                showHintUIPackageAndMinimap = false;
                canPackageBeDeliveredAtWrongNode = true;
                showUIPackages = true;
                randomizePackageValues = true;
                randomizeOrderUIPackages = true;
                break;
            case LevelDifficultyEnum.Easy1:
                Debug.Log("Set Easy 1 Mode stats");
                showAlreadyCorrectValueOnNode = true;
                showHintValueWhenWrongDelivered = true;
                showHintColorWhenDelivered = true;
                showHintUIPackageAndMinimap = true;
                canPackageBeDeliveredAtWrongNode = false;
                showUIPackages = true;
                randomizePackageValues = false;
                randomizeOrderUIPackages = false;
                break;
            case LevelDifficultyEnum.Easy2:
                Debug.Log("Set Easy 2 Mode stats");
                showAlreadyCorrectValueOnNode = true;
                showHintValueWhenWrongDelivered = true;
                showHintColorWhenDelivered = true;
                showHintUIPackageAndMinimap = true;
                canPackageBeDeliveredAtWrongNode = false;
                showUIPackages = true;
                randomizePackageValues = false;
                randomizeOrderUIPackages = false;
                break;
            case LevelDifficultyEnum.Hard1:
                Debug.Log("Set Hard 1 Mode stats");
                showAlreadyCorrectValueOnNode = false;
                showHintValueWhenWrongDelivered = false;
                showHintColorWhenDelivered = true;
                showHintUIPackageAndMinimap = true;
                canPackageBeDeliveredAtWrongNode = false;
                showUIPackages = true;
                randomizePackageValues = true;
                randomizeOrderUIPackages = true;
                break;
            case LevelDifficultyEnum.Hard2:
                Debug.Log("Set Hard 2 Mode stats");
                showAlreadyCorrectValueOnNode = false;
                showHintValueWhenWrongDelivered = false;
                showHintColorWhenDelivered = true;
                showHintUIPackageAndMinimap = true;
                canPackageBeDeliveredAtWrongNode = false;
                showUIPackages = false;
                randomizePackageValues = true;
                randomizeOrderUIPackages = true;
                break;
            default:
                Debug.LogWarning("Undefined Level Mode in DifficultyController!");
                break;
        }

        Debug.Log("showHintWhenWrongDelivered: " + showHintValueWhenWrongDelivered);
        Debug.Log("showUIPackages: " + showUIPackages);
        Debug.Log("randomizePackageValues: " + randomizePackageValues);
        Debug.Log("randomizeOrderUIPackages: " + randomizeOrderUIPackages);
    }

    public void IncreaseWrongDelivery()
    {
        countWrongDeliveries++;
    }

    public LevelDifficultyEnum DetermineDifficulty()
    {
        switch (currentLevelDifficulty)
        {
            case LevelDifficultyEnum.Tutorial:
                currentLevelDifficulty = LevelDifficultyEnum.Test;
                break;
            case LevelDifficultyEnum.Test:
                if (countWrongDeliveries > acceptableNumberOfWrongDeliveries)
                {
                    currentLevelDifficulty = LevelDifficultyEnum.Easy1;                    
                }
                else
                {
                    currentLevelDifficulty = LevelDifficultyEnum.Hard1;
                }
                break;
            case LevelDifficultyEnum.Easy1:
                currentLevelDifficulty = LevelDifficultyEnum.Easy2;
                break;
            case LevelDifficultyEnum.Hard1:
                currentLevelDifficulty = LevelDifficultyEnum.Hard2;
                break;
            default:
                Debug.LogWarning("Unable to determine difficulty!");
                break;
        }
        return currentLevelDifficulty;
    }
}
