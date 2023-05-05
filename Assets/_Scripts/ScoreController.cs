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

        if (SceneManager.isGamePaused)
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

    public void IncreaseScorePackageFirstTimeCorrectDelivered(Mailbox mailbox)
    {
        Debug.Log("Add score: Package first time correct delivered");
        startScore += scoreAmountFirstTimeCorrectDelivered;
        mailbox.ShowScorePopup(scoreAmountFirstTimeCorrectDelivered);
    }

    public void IncreaseScorePackageDelivered(Mailbox mailbox, int timesPackageWrongDeliveredCounter)
    {
        Debug.Log("Add score: Package correct delivered, after times: " + timesPackageWrongDeliveredCounter);
        int amount = Mathf.RoundToInt(scoreAmountFirstTimeCorrectDelivered - (timesPackageWrongDeliveredCounter * timesWrongDeliveredPenalty));
        startScore += amount;
        mailbox.ShowScorePopup(amount);
    }

    public void DecreaseScorePackageWrongDelivered(Mailbox mailbox)
    {
        Debug.Log("Remove score: Package wrong delivered");
        startScore -= scoreAmountWrongDelivered;
        mailbox.ShowScorePopup(-scoreAmountWrongDelivered);
    }
}
