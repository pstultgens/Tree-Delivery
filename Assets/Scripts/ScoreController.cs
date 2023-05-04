using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public static int currentScore;

    [SerializeField] public int startScore = 99999;
    [SerializeField] public int scoreDelayAmount = 1;

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

        DecreaseScore();
    }

    private void DecreaseScore()
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
}
