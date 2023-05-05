using UnityEngine;
using TMPro;

public class Package : MonoBehaviour
{
    [SerializeField] public GameObject minimapIcon;
    [SerializeField] public bool isCorrectDelivered;

    private UIController uiController;
    private GameObject player;
    private ScoreController scoreController;

    public int timesPackageWrongDeliveredCounter = 0;

    private void Awake()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        scoreController = FindObjectOfType<ScoreController>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (DifficultyController.Instance.showPackageOnMinimap)
        {
            ShowOnMinimap();
        }
    }

    public int Value()
    {
        TextMeshPro tmPro = GetComponentInChildren<TextMeshPro>();
        return int.Parse(tmPro.text);
    }

    public void Pickedup()
    {
        MusicController.Instance.PlayPickupSFX();

        uiController.PackagePickedup(Value());
        gameObject.SetActive(false);
    }

    public void Drop(Vector2 dropLocation)
    {
        MusicController.Instance.PlayDropSFX();

        transform.position = dropLocation;
        ShowOnMinimap();
        gameObject.SetActive(true);
    }

    public void CorrectDelivered(Mailbox mailbox)
    {
        isCorrectDelivered = true;

        if (timesPackageWrongDeliveredCounter == 0 && scoreController != null)
        {
            scoreController.IncreaseScorePackageFirstTimeCorrectDelivered(mailbox);
        }
        else if (scoreController != null)
        {
            scoreController.IncreaseScorePackageDelivered(mailbox, timesPackageWrongDeliveredCounter);
        }

        if (DifficultyController.Instance.showHintUIPackageAndMinimap)
        {
            uiController.PackageCorrectDelivered(Value());
        }
        // Set delivered package at players location
        transform.position = player.transform.position;

        this.gameObject.SetActive(false);
    }

    public void WrongDelivered()
    {
        isCorrectDelivered = false;

        if (DifficultyController.Instance.showHintUIPackageAndMinimap)
        {
            uiController.PackageWrongDelivered(Value());
        }
        // Set delivered package at players location
        transform.position = player.transform.position;

        this.gameObject.SetActive(false);
    }

    public void AddCounterWrongDelivered()
    {
        timesPackageWrongDeliveredCounter++;
    }

    private void ShowOnMinimap()
    {
        minimapIcon.SetActive(true);

        if (DifficultyController.Instance.showPackageValueOnMinimap)
        {
            TextMeshPro minimapIconText = minimapIcon.GetComponentInChildren<TextMeshPro>();
            minimapIconText.text = Value().ToString();
        }
    }


}