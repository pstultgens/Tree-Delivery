using UnityEngine;
using TMPro;

public class Package : MonoBehaviour
{
    [SerializeField] public GameObject minimapIcon;
    public bool isCorrectDelivered;
    public bool isDelivered;

    private UIController uiController;
    private ScoreController scoreController;

    public int timesPackageWrongValueDeliveredCounter = 0;

    private bool isAlreadyFirstTimeCorrectDelivered;

    private void Awake()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
    }

    private void Start()
    {
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

        if(timesPackageWrongValueDeliveredCounter == 0 && !isAlreadyFirstTimeCorrectDelivered)
        {
            isAlreadyFirstTimeCorrectDelivered = true;

            if (scoreController == null)
            {
                scoreController = FindObjectOfType<ScoreController>();
            }

            scoreController.IncreaseFirstTimeCorrectDeliveredCounter();
        }

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

        this.gameObject.SetActive(false);
    }

    public void AddCounterWrongValueDelivered()
    {
        timesPackageWrongValueDeliveredCounter++;
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