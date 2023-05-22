using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public static int currentScore;

    [SerializeField] public int startScore = 999;
    [SerializeField] public int scoreDelayAmount = 1;

    [SerializeField] public int scoreAmountFirstTimeCorrectDelivered = 20;   
    [SerializeField] public int scoreAmountWrongDelivered = 15;
    [SerializeField] public int timesWrongDeliveredPenalty = 5;


    private TextMeshProUGUI scoringText;
    private float timer;

    private DeliveryController deliveryController;

    private void Start()
    {
        deliveryController = FindObjectOfType<DeliveryController>();
        scoringText = GetComponent<TextMeshProUGUI>();
        currentScore = startScore;
    }

    private void Update()
    {
        if (deliveryController.AllPackagesCorrectDelivered())
        {
            return;
        }

        if (SceneManager.isGamePaused || SceneManager.isCountingDown)
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
            currentScore -= 1;
        }

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoringText.text = currentScore.ToString();
    }

    public void IncreaseScorePackageFirstTimeCorrectDelivered(Spot spot)
    {
        Debug.Log("Add score: Package first time correct delivered");
        currentScore += scoreAmountFirstTimeCorrectDelivered;
        UpdateScoreText();
        spot.ShowScorePopup(scoreAmountFirstTimeCorrectDelivered);
    }

    public void IncreaseScorePackageDelivered(Spot spot, int timesPackageWrongDeliveredCounter)
    {
        Debug.Log("Add score: Package correct delivered, after times: " + timesPackageWrongDeliveredCounter);
        int amount = Mathf.RoundToInt(scoreAmountFirstTimeCorrectDelivered - (timesPackageWrongDeliveredCounter * timesWrongDeliveredPenalty));
        currentScore += amount;
        UpdateScoreText();
        spot.ShowScorePopup(amount);
    }

    public void DecreaseScorePackageWrongDelivered(Spot spot)
    {
        Debug.Log("Remove score: Package wrong delivered");
        currentScore -= scoreAmountWrongDelivered;
        UpdateScoreText();
        spot.ShowScorePopup(-scoreAmountWrongDelivered);
    }
}
