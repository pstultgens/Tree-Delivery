using UnityEngine;
using TMPro;

public class Package : MonoBehaviour
{
    [SerializeField] public GameObject minimapIcon;
    public bool isCorrectDelivered;
    public bool isDelivered;

    private UIController uiController;
    private ScoreController scoreController;
    private GameObject player;

    public int timesPackageWrongDeliveredCounter = 0;

    private void Awake()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        scoreController = FindObjectOfType<ScoreController>();
        ShowOnMinimap();
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

        if (HintController.Instance.showHintUIPackageAndMinimapNode)
        {
            uiController.PackageCorrectDelivered(Value());
        }

        if(timesPackageWrongDeliveredCounter == 0)
        {
            scoreController.IncreaseFirstTimeCorrectDeliveredCounter();
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
        if (HintController.Instance.showPackageOnMinimap)
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
}