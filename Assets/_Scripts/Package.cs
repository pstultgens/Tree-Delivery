using UnityEngine;
using TMPro;

public class Package : MonoBehaviour
{
    [SerializeField] public GameObject minimapIcon;
    public bool isCorrectDelivered;
    public bool isDelivered;

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

        if (HintController.Instance.showPackageOnMinimap)
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
        isCorrectDelivered = false;
        isDelivered = false;

        MusicController.Instance.PlayPickupSFX();

        uiController.PackagePickedup(Value());
        gameObject.SetActive(false);        
    }

    public void Drop(Vector2 dropLocation)
    {
        isCorrectDelivered = false;
        isDelivered = false;

        MusicController.Instance.PlayDropSFX();

        transform.position = dropLocation;
        ShowOnMinimap();
        gameObject.SetActive(true);        
    }

    public void CorrectDelivered(Spot spot)
    {
        isCorrectDelivered = true;
        isDelivered = true;

        if (!HintController.Instance.canPackageBeDeliveredAtWrongNode)
        {
            if (timesPackageWrongDeliveredCounter == 0 && scoreController != null)
            {
                scoreController.IncreaseScorePackageFirstTimeCorrectDelivered(spot);
            }
            else if (scoreController != null)
            {
                scoreController.IncreaseScorePackageDelivered(spot, timesPackageWrongDeliveredCounter);
            }
        }

        if (HintController.Instance.showHintUIPackageAndMinimapNode)
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
        isDelivered = true;

        if (HintController.Instance.showHintUIPackageAndMinimapNode)
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
        TextMeshPro minimapIconText = minimapIcon.GetComponentInChildren<TextMeshPro>();
        if (HintController.Instance.showPackageValueOnMinimap)
        {

            minimapIconText.text = Value().ToString();
        }
        else
        {
            minimapIconText.text = "";
        }
    }


}