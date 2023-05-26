using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public static int countWrongDeliveries = 0;
    public static int countFirstTimeCorrectDeliveries = 0;

    [Header("Time Bonus")]
    [SerializeField] float maxTime = 180f;
    [SerializeField] float maxBonus = 1000f;

    [Header("Delivery Bonus/Penalty")]
    [SerializeField] int flawlessDeliveryBonus = 200;
    [SerializeField] int deliveryPenalty = 100;

    private TimeController timeController;

    private void Start()
    {
        timeController = FindObjectOfType<TimeController>();

        countWrongDeliveries = 0;
        countFirstTimeCorrectDeliveries = 0;
    }

    private void Update()
    {
        if (timeController == null)
        {
            timeController = FindObjectOfType<TimeController>();
        }
    }

    public void IncreaseWrongDeliveredCounter()
    {
        countWrongDeliveries++;
    }

    public void IncreaseFirstTimeCorrectDeliveredCounter()
    {
        countFirstTimeCorrectDeliveries++;
    }

    public int DetermineTimeBonus()
    {
        float timeBonus = 0f;

        int elapsedTime = timeController.GetElapsedTimeInSeconds();

        // Calculate the time bonus based on the elapsed time
        if (elapsedTime <= maxTime)
        {
            float timeRatio = 1f - (elapsedTime / maxTime);
            timeBonus = timeRatio * maxBonus;
        }

        return Mathf.RoundToInt(timeBonus);
    }

    public int DetermineFlawlessDeliveryBonus()
    {
        return countFirstTimeCorrectDeliveries * flawlessDeliveryBonus;
    }

    public int DetermineDeliveryPenalty()
    {
        return countWrongDeliveries * deliveryPenalty;
    }

    public int DetermineTotalScore()
    {
        return DetermineTimeBonus() + DetermineFlawlessDeliveryBonus() - DetermineDeliveryPenalty();
    }
}
