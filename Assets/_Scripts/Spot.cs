using System.Collections;
using UnityEngine;
using TMPro;

public class Spot : MonoBehaviour
{
    [Header("Tree Stats")]
    [SerializeField] public bool isRoot;
    [SerializeField] public Spot parent;
    [SerializeField] public Spot leftChild;
    [SerializeField] public Spot rightChild;

    [Header("Stats")]
    [SerializeField] public int correctValue;
    [SerializeField] public float wrongDeliveryDelay = 2.0f;
    [SerializeField] Color32 correctDeliverdColor = new Color32(1, 1, 1, 1);
    [SerializeField] Color32 wrongDeliverdColor = new Color32(1, 1, 1, 1);
    [SerializeField] Package initialWrongReceivedPackage;

    [Header("Minimap Icons")]
    [SerializeField] public GameObject minimapNode;
    [SerializeField] public GameObject minimapEdgeLeft;
    [SerializeField] public GameObject minimapEdgeRight;

    [Header("Edge obstacles")]
    [SerializeField] public GameObject edgeLeftClosed;
    [SerializeField] public GameObject edgeRightClosed;

    [Header("VFX")]
    [SerializeField] public ParticleSystem correctDeliveredVFX;
    [SerializeField] public ParticleSystem wrongDeliveredVFX;
    [SerializeField] public ParticleSystem neutralDeliveredVFX;

    [Header("SFX Audio sources")]
    [SerializeField] public AudioSource correctDeliveredAudioSource;
    [SerializeField] public AudioSource wrongDeliveredAudioSource;
    [SerializeField] public AudioSource neutralDeliveredAudioSource;

    public bool hasReceivedCorrectPackage;
    public bool hasReceivedPackage;
    public int receivedPackageValue;
    //public bool isLocked;

    Color32 defaultSpotColor;
    Color32 defaultMinimapNodeColor;

    private bool isWrongDeliveryColorCoroutineRunning;
    private bool isShowHintValueCoroutineRunning;

    private SpriteRenderer spriteRenderer;
    private TextMeshPro textMeshPro;

    private TextMeshPro minimapNodeTextMeshPro;
    private SpriteRenderer minimapNodeSpriteRenderer;

    private void Awake()
    {
        textMeshPro = GetComponentInChildren<TextMeshPro>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        minimapNodeTextMeshPro = minimapNode.GetComponentInChildren<TextMeshPro>();
        minimapNodeSpriteRenderer = minimapNode.GetComponent<SpriteRenderer>();

        defaultSpotColor = spriteRenderer.color;
        defaultMinimapNodeColor = minimapNodeSpriteRenderer.color;
    }

    private void Start()
    {
        if (initialWrongReceivedPackage != null)
        {
            InitialWrongReceivedPackage();
        }
    }

    private void InitialWrongReceivedPackage()
    {
        hasReceivedPackage = true;
        hasReceivedCorrectPackage = false;
        receivedPackageValue = initialWrongReceivedPackage.Value();

        textMeshPro.text = initialWrongReceivedPackage.Value().ToString();
        textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 1f);

        minimapNodeTextMeshPro.text = initialWrongReceivedPackage.Value().ToString();
        minimapNodeTextMeshPro.color = new Color(minimapNodeTextMeshPro.color.r, minimapNodeTextMeshPro.color.g, minimapNodeTextMeshPro.color.b, 1f);

        initialWrongReceivedPackage.WrongDelivered();
    }

    public bool CanDeliverPackage()
    {
        if (isRoot || parent.hasReceivedPackage)
        {
            return true;
        }
        return false;
    }

    // Bubbles up to all parents
    public void ShowHintCannotDeliverPackage()
    {
        parent.ShowWrongDeliveryHintColor();

        Spot nextParent = parent.parent;

        if ((nextParent != null || nextParent.isRoot) && !nextParent.hasReceivedPackage)
        {
            nextParent.ShowWrongDeliveryHintColor();
        }
    }

    public bool CanRemovePackage()
    {
        // No package to remove
        if (!hasReceivedPackage)
        {
            return false;
        }

        // Has children value
        if ((leftChild != null && leftChild.hasReceivedPackage)
            || (rightChild != null && rightChild.hasReceivedPackage))
        {
            return false;
        }
        return true;
    }

    public void ShowHintCannotRemovePackage()
    {
        if (leftChild != null && leftChild.hasReceivedPackage)
        {
            leftChild.ShowWrongDeliveryHintColor();
        }

        if (rightChild != null && rightChild.hasReceivedPackage)
        {
            rightChild.ShowWrongDeliveryHintColor();
        }
    }

    // Show hint color when all packages are delivered
    public void ShowHintColor()
    {
        if (hasReceivedCorrectPackage)
        {
            //isLocked = true;
            spriteRenderer.color = correctDeliverdColor;
            minimapNodeSpriteRenderer.color = correctDeliverdColor;
        }
        else
        {
            spriteRenderer.color = wrongDeliverdColor;
            minimapNodeSpriteRenderer.color = wrongDeliverdColor;
        }
    }

    public void ShowCorrectValue()
    {
        textMeshPro.text = correctValue.ToString();
        textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 1f);

        minimapNodeTextMeshPro.text = correctValue.ToString();
        minimapNodeTextMeshPro.color = new Color(minimapNodeTextMeshPro.color.r, minimapNodeTextMeshPro.color.g, minimapNodeTextMeshPro.color.b, 1f);
    }

    public void ShowHintCorrectValue()
    {
        textMeshPro.text = correctValue.ToString();
        textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 0.5f);

        minimapNodeTextMeshPro.text = correctValue.ToString();
        minimapNodeTextMeshPro.color = new Color(minimapNodeTextMeshPro.color.r, minimapNodeTextMeshPro.color.g, minimapNodeTextMeshPro.color.b, 0.5f);
    }

    public void ShowDeliveredValue()
    {
        textMeshPro.text = receivedPackageValue.ToString();
        textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 1f);

        minimapNodeTextMeshPro.text = receivedPackageValue.ToString();
        minimapNodeTextMeshPro.color = new Color(minimapNodeTextMeshPro.color.r, minimapNodeTextMeshPro.color.g, minimapNodeTextMeshPro.color.b, 1f);
    }

    public void ShowWrongDeliveryHintValue()
    {
        if (!isShowHintValueCoroutineRunning)
        {
            StartCoroutine(ShowHintValueCoroutine());
        }
    }

    public void ShowWrongDeliveryHintColor()
    {
        if (!isWrongDeliveryColorCoroutineRunning)
        {
            StartCoroutine(WrongDeliveryColorCoroutine());
        }
    }

    public void HideValue()
    {
        textMeshPro.text = "?";
        textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 0.5f);

        minimapNodeTextMeshPro.text = "?";
        minimapNodeTextMeshPro.color = new Color(minimapNodeTextMeshPro.color.r, minimapNodeTextMeshPro.color.g, minimapNodeTextMeshPro.color.b, 0.5f);
    }

    public void PickupPackage()
    {
        HideValue();
        hasReceivedPackage = false;
        hasReceivedCorrectPackage = false;
        receivedPackageValue = 0;

        spriteRenderer.color = defaultSpotColor;
        minimapNodeSpriteRenderer.color = defaultMinimapNodeColor;
    }

    public void CorrectPackageReceived()
    {
        hasReceivedCorrectPackage = true;
        hasReceivedPackage = true;
        receivedPackageValue = correctValue;
        ShowDeliveredValue();

        if (HintController.Instance.showHintColorWhenDelivered)
        {
            //isLocked = true;
            PlayCorrectDeliveredSFX();
            PlayCorrectDeliveredVFX();
            spriteRenderer.color = correctDeliverdColor;
            minimapNodeSpriteRenderer.color = correctDeliverdColor;
        }
        else if (HintController.Instance.canPackageBeDeliveredAtWrongNode)
        {
            PlayNeutralDeliveredSFX();
            PlayNeutralDeliveredVFX();
        }

        UpdateMinimap();
        OpenToNextNode();
    }

    public void WrongPackageReceived(int packageValue)
    {
        hasReceivedPackage = true;
        hasReceivedCorrectPackage = false;
        receivedPackageValue = packageValue;
        ShowDeliveredValue();

        if (HintController.Instance.showHintColorWhenDelivered)
        {
            PlayWrongDeliveredSFX();
            PlayWrongDeliveredVFX();
            spriteRenderer.color = wrongDeliverdColor;
            minimapNodeSpriteRenderer.color = wrongDeliverdColor;
        }
        else
        {
            PlayNeutralDeliveredSFX();
            PlayNeutralDeliveredVFX();
        }
    }


    private void UpdateMinimap()
    {
        // Update Minimap Node
        minimapNode.GetComponentInChildren<TextMeshPro>().text = correctValue.ToString();

        if (HintController.Instance.showHintUIPackageAndMinimapNode)
        {
            minimapNode.GetComponent<SpriteRenderer>().color = correctDeliverdColor;

            // Update Minimap Edges
            if (minimapEdgeLeft != null)
            {
                minimapEdgeLeft.GetComponent<SpriteRenderer>().color = correctDeliverdColor;
            }

            if (minimapEdgeRight != null)
            {
                minimapEdgeRight.GetComponent<SpriteRenderer>().color = correctDeliverdColor;
            }
        }
    }

    private void OpenToNextNode()
    {
        if (edgeLeftClosed != null)
        {
            Destroy(edgeLeftClosed);
        }

        if (edgeRightClosed != null)
        {
            Destroy(edgeRightClosed);
        }
    }

    private IEnumerator WrongDeliveryColorCoroutine()
    {
        isWrongDeliveryColorCoroutineRunning = true;

        PlayWrongDeliveredSFX();
        PlayWrongDeliveredVFX();
        Color32 currentSpotColor = spriteRenderer.color;
        Color32 currentMinimapSpotColor = minimapNodeSpriteRenderer.color;

        spriteRenderer.color = wrongDeliverdColor;
        minimapNodeSpriteRenderer.color = wrongDeliverdColor;

        yield return new WaitForSeconds(wrongDeliveryDelay);

        spriteRenderer.color = currentSpotColor;
        minimapNodeSpriteRenderer.color = currentMinimapSpotColor;

        isWrongDeliveryColorCoroutineRunning = false;
    }

    private IEnumerator ShowHintValueCoroutine()
    {
        isShowHintValueCoroutineRunning = true;

        ShowCorrectValue();
        yield return new WaitForSeconds(wrongDeliveryDelay);

        if (HintController.Instance.showAlreadyCorrectValueOnNode)
        {
            ShowHintCorrectValue();
        }
        else
        {
            HideValue();
        }

        isShowHintValueCoroutineRunning = false;
    }

    private void PlayCorrectDeliveredVFX()
    {
        FeedbacksManager.Instance.spotDelivered.PlayFeedbacks();
        correctDeliveredVFX.Play();
    }

    private void PlayWrongDeliveredVFX()
    {
        FeedbacksManager.Instance.spotDelivered.PlayFeedbacks();
        wrongDeliveredVFX.Play();
    }

    private void PlayNeutralDeliveredVFX()
    {
        FeedbacksManager.Instance.spotDelivered.PlayFeedbacks();
        neutralDeliveredVFX.Play();
    }

    private void PlayCorrectDeliveredSFX()
    {
        correctDeliveredAudioSource.Play();
    }

    private void PlayWrongDeliveredSFX()
    {
        wrongDeliveredAudioSource.Play();
    }

    private void PlayNeutralDeliveredSFX()
    {
        neutralDeliveredAudioSource.Play();
    }
}
