using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Mailbox : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public int correctValue;
    [SerializeField] public float wrongDeliveryDelay = 2.0f;
    [SerializeField] Color32 correctDeliverdColor = new Color32(1, 1, 1, 1);
    [SerializeField] Color32 wrongDeliverdColor = new Color32(1, 1, 1, 1);

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

    [Header("SFX Audio sources")]
    [SerializeField] public AudioSource correctDeliveredAudioSource;
    [SerializeField] public AudioSource wrongDeliveredAudioSource;

    public bool hasReceivedCorrectPackage;
    public bool hasReceivedPackage;
    public int receivedPackageValue;

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
    }

    public void ShowCorrectValue()
    {
        textMeshPro.text = correctValue.ToString();
        textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 1f);
    }

    public void ShowHintCorrectValue()
    {
        textMeshPro.text = correctValue.ToString();
        textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 0.6f);

        minimapNodeTextMeshPro.text = correctValue.ToString();
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
    }

    public void PickupPackage()
    {
        HideValue();
        hasReceivedPackage = false;
        receivedPackageValue = 0;
    }

    public void CorrectPackageReceived()
    {
        hasReceivedCorrectPackage = true;
        hasReceivedPackage = true;
        receivedPackageValue = correctValue;
        ShowCorrectValue();

        if (DifficultyController.Instance.showHintColorWhenDelivered)
        {
            PlayCorrectDeliveredSFX();
            PlayCorrectDeliveredVFX();
            spriteRenderer.color = correctDeliverdColor;
        }

        UpdateMinimap();
        OpenToNextNode();
    }

    public void WrongPackageReceived(int packageValue)
    {
        hasReceivedPackage = true;
        hasReceivedCorrectPackage = false;
        receivedPackageValue = packageValue;
        textMeshPro.text = packageValue.ToString();
    }

    private void UpdateMinimap()
    {
        // Update Minimap Node
        minimapNode.GetComponentInChildren<TextMeshPro>().text = correctValue.ToString();

        if (DifficultyController.Instance.showHintUIPackageAndMinimap)
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

        Color32 defaultMailboxColor = spriteRenderer.color;
        Color32 defaultMinimapNodeColor = minimapNodeSpriteRenderer.color;

        spriteRenderer.color = wrongDeliverdColor;
        minimapNodeSpriteRenderer.color = wrongDeliverdColor;

        yield return new WaitForSeconds(wrongDeliveryDelay);

        spriteRenderer.color = defaultMailboxColor;
        minimapNodeSpriteRenderer.color = defaultMinimapNodeColor;

        isWrongDeliveryColorCoroutineRunning = false;
    }

    private IEnumerator ShowHintValueCoroutine()
    {
        isShowHintValueCoroutineRunning = true;

        ShowCorrectValue();
        minimapNodeTextMeshPro.text = correctValue.ToString();
        yield return new WaitForSeconds(wrongDeliveryDelay);
        HideValue();

        if (DifficultyController.Instance.showAlreadyCorrectValueOnNode)
        {
            minimapNodeTextMeshPro.text = correctValue.ToString();
        }
        else
        {
            minimapNodeTextMeshPro.text = "?";
        }

        isShowHintValueCoroutineRunning = false;
    }

    private void PlayCorrectDeliveredVFX()
    {
        correctDeliveredVFX.Play();
    }

    private void PlayWrongDeliveredVFX()
    {
        wrongDeliveredVFX.Play();
    }

    private void PlayCorrectDeliveredSFX()
    {
        correctDeliveredAudioSource.Play();
    }

    private void PlayWrongDeliveredSFX()
    {
        wrongDeliveredAudioSource.Play();
    }
}
