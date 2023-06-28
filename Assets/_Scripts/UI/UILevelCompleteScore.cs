using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Analytics;

public class UILevelCompleteScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI completionTimeText;
    [SerializeField] TextMeshProUGUI incorrectValueDeliveriesText;
    [SerializeField] TextMeshProUGUI incorrectHasNoParentDeliveriesText;
    [SerializeField] TextMeshProUGUI incorrectHasChildDeliveriesText;
    [SerializeField] TextMeshProUGUI perfectDeliveriesText;

    [SerializeField] TextMeshProUGUI timeBonusText;
    [SerializeField] TextMeshProUGUI flawlessDeliveryBonusText;
    [SerializeField] TextMeshProUGUI deliveryPenaltyText;
    [SerializeField] TextMeshProUGUI totalScoreText;

    private TimeController timeController;
    private ScoreController scoreController;
    private bool showResults = false;

    private void Start()
    {
        timeController = FindObjectOfType<TimeController>();
        scoreController = FindObjectOfType<ScoreController>();
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            if (showResults)
            {
                return;
            }
            showResults = true;

            int countWrongValueDeliveries = ScoreController.countWrongValueDeliveries;
            int countCannotDeliverHasNoParentDeliveries = ScoreController.countCannotDeliverHasNoParentDeliveries;
            int countCannotRemoveHasChildDeliveries = ScoreController.countCannotRemoveHasChildDeliveries;
            int countFirstTimeCorrectDeliveries = ScoreController.countFirstTimeCorrectDeliveries;

            completionTimeText.text = timeController.GetFormattedTime();
            incorrectValueDeliveriesText.text = countWrongValueDeliveries.ToString();
            incorrectHasNoParentDeliveriesText.text = countCannotDeliverHasNoParentDeliveries.ToString();
            incorrectHasChildDeliveriesText.text = countCannotRemoveHasChildDeliveries.ToString();
            perfectDeliveriesText.text = countFirstTimeCorrectDeliveries.ToString();

            ShowTimeBonus();
            ShowFlawlessDeliveryBonus();
            ShowDeliveryPenalty();

            totalScoreText.text = scoreController.DetermineTotalScore().ToString();


            AnalyticsResult analyticsResult = Analytics.CustomEvent(
                "LevelFinished",
                new Dictionary<string, object>
                {
                    { "levelName", HintController.Instance.currentLevel.GetName()},
                    { "time", timeController.GetFormattedTime() },
                    { "incorrectValueDeliveries", countWrongValueDeliveries },
                    { "incorrectHasNoParentDeliveries", countCannotDeliverHasNoParentDeliveries },
                    { "incorrectHasChildDeliveries", countCannotRemoveHasChildDeliveries },
                    { "perfectDeliveries", countFirstTimeCorrectDeliveries},
            });

            Debug.Log("Analytics Enabled: " + Analytics.enabled);
            Debug.Log("AnalyticsResult LevelFinished: " + analyticsResult);
        }
    }

    private void ShowTimeBonus()
    {
        int timeBonus = scoreController.DetermineTimeBonus();
        if (timeBonus > 0)
        {
            timeBonusText.text = "+" + timeBonus.ToString();
        }
        else
        {
            timeBonusText.transform.parent.gameObject.SetActive(false);
        }
    }

    private void ShowFlawlessDeliveryBonus()
    {
        int flawlessDeliveryBonus = scoreController.DetermineFlawlessDeliveryBonus();
        if (flawlessDeliveryBonus > 0)
        {
            flawlessDeliveryBonusText.text = "+" + flawlessDeliveryBonus.ToString();
        }
        else
        {
            flawlessDeliveryBonusText.transform.parent.gameObject.SetActive(false);
        }
    }

    private void ShowDeliveryPenalty()
    {
        int deliveryPenalty = scoreController.DetermineDeliveryPenalty();
        if (deliveryPenalty > 0)
        {
            deliveryPenaltyText.text = "-" + deliveryPenalty.ToString();
        }
        else
        {
            deliveryPenaltyText.transform.parent.gameObject.SetActive(false);
        }
    }
}
