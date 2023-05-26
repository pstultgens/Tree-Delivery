using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UILevelCompleteScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI completionTimeText;
    [SerializeField] TextMeshProUGUI incorrectDeliveriesText;
    [SerializeField] TextMeshProUGUI perfectDeliveriesText;

    [SerializeField] TextMeshProUGUI timeBonusText;
    [SerializeField] TextMeshProUGUI flawlessDeliveryBonusText;
    [SerializeField] TextMeshProUGUI deliveryPenaltyText;
    [SerializeField] TextMeshProUGUI totalScoreText;

    private TimeController timeController;
    private ScoreController scoreController;

    private void Start()
    {
        timeController = FindObjectOfType<TimeController>();
        scoreController = FindObjectOfType<ScoreController>();
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            completionTimeText.text = timeController.GetFormattedTime();
            incorrectDeliveriesText.text = ScoreController.countWrongDeliveries.ToString();
            perfectDeliveriesText.text = ScoreController.countFirstTimeCorrectDeliveries.ToString();

            ShowTimeBonus();
            ShowFlawlessDeliveryBonus();
            ShowDeliveryPenalty();

            totalScoreText.text = scoreController.DetermineTotalScore().ToString();
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
