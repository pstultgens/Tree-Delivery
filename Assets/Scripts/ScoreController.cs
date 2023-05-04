using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public static int currentScore;

    [SerializeField] public int startScore = 999;
    [SerializeField] public int scoreDelayAmount = 1;

    [SerializeField] public int addScoreFirstTimeCorrectDelivered = 20;
    [SerializeField] public int removeScoreWrongDelivered = 15;


    private TextMeshProUGUI scoringText;
    private float timer;

    private DeliveryController deliveryController;

    private void Awake()
    {
        deliveryController = FindObjectOfType<DeliveryController>();
        scoringText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (deliveryController.allPackagesCorrectDelivered)
        {
            return;
        }

        DecreaseScoreOverTime();
    }

    private void DecreaseScoreOverTime()
    {
        timer += Time.deltaTime;

        if (timer >= scoreDelayAmount)
        {
            timer = 0f;
            startScore -= 1;
        }

        scoringText.text = startScore.ToString();
        currentScore = startScore;
    }

    public void AddScorePackageFirstTimeCorrectDelivered()
    {
        Debug.Log("Add score: Package first time correct delivered");
        startScore += addScoreFirstTimeCorrectDelivered;
    }

    public void RemoveScorePackageWrongDelivered()
    {
        Debug.Log("Remove score: Package wrong delivered");
        startScore -= removeScoreWrongDelivered;
    }
}
