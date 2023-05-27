using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITestLevelCompleteScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI completionTimeText;
    [SerializeField] TextMeshProUGUI incorrectDeliveriesText;
    [SerializeField] TextMeshProUGUI perfectDeliveriesText;

    [SerializeField] GameObject testPassedGameObject;
    [SerializeField] GameObject testFailedGameObject;

    private TimeController timeController;

    private void Start()
    {
        timeController = FindObjectOfType<TimeController>();
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            int countWrongDeliveries = ScoreController.countWrongDeliveries;

            completionTimeText.text = timeController.GetFormattedTime();
            incorrectDeliveriesText.text = countWrongDeliveries.ToString();
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
