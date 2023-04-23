using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyController : Singleton
{
    [SerializeField] public LevelDifficultyEnum currentLevelDifficulty = LevelDifficultyEnum.Tutorial;
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

    private void Start()
    {
        UpdateDifficultyStats();
    }

    public void IncreaseWrongDelivery()
    {
        countWrongDeliveries++;
    }

    public LevelDifficultyEnum DetermineDifficulty()
    {
        Debug.Log("Current Level Difficulty: " + currentLevelDifficulty.ToString());
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
        Debug.Log("Next Determined Level Difficulty: " + currentLevelDifficulty.ToString());

        UpdateDifficultyStats();

        return currentLevelDifficulty;
    }

    private void UpdateDifficultyStats()
    {
        Debug.Log("Update Difficulty Stats");

        switch (currentLevelDifficulty)
        {
            case LevelDifficultyEnum.Tutorial:
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
            case LevelDifficultyEnum.Test:
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
            case LevelDifficultyEnum.Easy1:
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
            case LevelDifficultyEnum.Easy2:
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
            case LevelDifficultyEnum.Easy3:
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
            case LevelDifficultyEnum.Hard1:
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
            case LevelDifficultyEnum.Hard2:
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
