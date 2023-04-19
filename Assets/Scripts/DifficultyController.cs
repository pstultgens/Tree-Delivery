using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    public static LevelModeEnum currentLevelMode = LevelModeEnum.Tutorial;
    public static bool showHintWhenWrongDelivered = true;
    public static bool showUIPackages = true;
    public static bool randomizeOrderUIPackages = false;
    public static bool randomizePackageValues = true;

    [SerializeField] public int acceptableNumberOfWrongDeliveries = 3;
    [SerializeField] public int countWrongDeliveries = 0;

    private void Awake()
    {
        Debug.Log("Current Level Difficulty: " + currentLevelMode.ToString());

        switch (currentLevelMode)
        {
            case LevelModeEnum.Test:
                Debug.Log("Set Test Mode stats");
                showHintWhenWrongDelivered = false;
                showUIPackages = false;
                randomizePackageValues = true;
                randomizeOrderUIPackages = true;

                break;
            case LevelModeEnum.Easy:
                Debug.Log("Set Easy Mode stats");
                showHintWhenWrongDelivered = true;
                showUIPackages = true;
                randomizePackageValues = false;
                randomizeOrderUIPackages = true;
                break;
            case LevelModeEnum.Hard:
                Debug.Log("Set Hard Mode stats");
                showHintWhenWrongDelivered = false;
                showUIPackages = false;
                randomizePackageValues = true;
                randomizeOrderUIPackages = true;
                break;
            default:
                Debug.LogWarning("Undefined Level Mode in DifficultyController!");
                break;
        }

        Debug.Log("showHintWhenWrongDelivered: " + showHintWhenWrongDelivered);
        Debug.Log("showUIPackages: " + showUIPackages);
        Debug.Log("randomizePackageValues: " + randomizePackageValues);
        Debug.Log("randomizeOrderUIPackages: " + randomizeOrderUIPackages);
    }


    public void IncreaseWrongDelivery()
    {
        countWrongDeliveries++;
    }

    public void DetermineDifficulty()
    {
        if (countWrongDeliveries <= acceptableNumberOfWrongDeliveries)
        {
            currentLevelMode = LevelModeEnum.Hard;
        }
        else
        {
            currentLevelMode = LevelModeEnum.Easy;
        }
    }
}
