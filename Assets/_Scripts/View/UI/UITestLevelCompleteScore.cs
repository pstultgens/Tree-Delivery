using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Analytics;

public class UITestLevelCompleteScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI completionTimeText;
    [SerializeField] TextMeshProUGUI incorrectValueDeliveriesText;
    [SerializeField] TextMeshProUGUI incorrectHasNoParentDeliveriesText;
    [SerializeField] TextMeshProUGUI incorrectHasChildDeliveriesText;
    [SerializeField] TextMeshProUGUI perfectDeliveriesText;

    [SerializeField] GameObject testPassedGameObject;
    [SerializeField] GameObject testFailedGameObject;

    private TimeController timeController;
    private bool showResults = false;

    private void Start()
    {
        timeController = FindObjectOfType<TimeController>();
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
            bool testLevelPassed;

            completionTimeText.text = timeController.GetFormattedTime();
            incorrectValueDeliveriesText.text = countWrongValueDeliveries.ToString();
            incorrectHasNoParentDeliveriesText.text = countCannotDeliverHasNoParentDeliveries.ToString();
            incorrectHasChildDeliveriesText.text = countCannotRemoveHasChildDeliveries.ToString();
            perfectDeliveriesText.text = countFirstTimeCorrectDeliveries.ToString();

            if (HintController.Instance.DetermineNextLevel().Equals(LevelEnum.Easy1))
            {
                testFailedGameObject.SetActive(true);
                testLevelPassed = false;
            }
            else
            {
                testPassedGameObject.SetActive(true);
                testLevelPassed = true;
            }

            // Analytics
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "time", timeController.GetFormattedTime() },
                { "passed", testLevelPassed},
                { "incorrectValueDeliveries", countWrongValueDeliveries },
                { "incorrectHasNoParentDeliveries", countCannotDeliverHasNoParentDeliveries },
                { "incorrectHasChildDeliveries", countCannotRemoveHasChildDeliveries },
                { "perfectDeliveries", countFirstTimeCorrectDeliveries},
            };
            AnalyticsService.Instance.CustomData("TestLevelFinished", parameters);
            AnalyticsService.Instance.Flush();
        }
    }
}
