using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    private bool showResults;

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

            completionTimeText.text = timeController.GetFormattedTime();
            incorrectValueDeliveriesText.text = countWrongValueDeliveries.ToString();
            incorrectHasNoParentDeliveriesText.text = countCannotDeliverHasNoParentDeliveries.ToString();
            incorrectHasChildDeliveriesText.text = countCannotRemoveHasChildDeliveries.ToString();
            perfectDeliveriesText.text = ScoreController.countFirstTimeCorrectDeliveries.ToString();

            if (HintController.Instance.DetermineNextLevel().Equals(LevelEnum.Easy1))
            {
                testFailedGameObject.SetActive(true);
            }
            else
            {
                testPassedGameObject.SetActive(true);
            }
        }
    }
}
