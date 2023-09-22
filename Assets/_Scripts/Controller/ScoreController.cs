using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public static int countWrongValueDeliveries = 0;
    public static int countCannotDeliverHasNoParentDeliveries = 0;
    public static int countCannotRemoveHasChildDeliveries = 0;
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

        countWrongValueDeliveries = 0;
        countCannotDeliverHasNoParentDeliveries = 0;
        countCannotRemoveHasChildDeliveries = 0;
        countFirstTimeCorrectDeliveries = 0;
    }

    private void Update()
    {
        if (timeController == null)
        {
            timeController = FindObjectOfType<TimeController>();
        }
    }

    public void IncreaseWrongValueDeliveredCounter()
    {
        countWrongValueDeliveries++;
    }

    public void IncreaseCannotDeliverHasNoParentCounter()
    {
        countCannotDeliverHasNoParentDeliveries++;
    }

    public void IncreaseCannotRemoveHasChildCounter()
    {
        countCannotRemoveHasChildDeliveries++;
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
        int penaltyWrongValueDeliveries = countWrongValueDeliveries * deliveryPenalty;
        int penaltyNoParentDeliveries = countCannotDeliverHasNoParentDeliveries * deliveryPenalty;
        int penaltyHasChildDeliveries = countCannotRemoveHasChildDeliveries * deliveryPenalty;

        return penaltyWrongValueDeliveries + penaltyNoParentDeliveries + penaltyHasChildDeliveries;
    }

    public int DetermineTotalScore()
    {
        return DetermineTimeBonus() + DetermineFlawlessDeliveryBonus() - DetermineDeliveryPenalty();
    }
}
