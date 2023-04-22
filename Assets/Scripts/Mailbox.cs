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

    public bool hasReceivedCorrectPackage;
    public bool hasReceivedPackage;
    public int receivedPackageValue;

    private SpriteRenderer spriteRenderer;
    private TextMeshPro textMeshPro;

    private TextMeshPro minimapNodeTextMeshPro;
    private SpriteRenderer minimapNodeSpriteRenderer;

    private MusicController musicController;


    private void Awake()
    {
        textMeshPro = GetComponentInChildren<TextMeshPro>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        minimapNodeTextMeshPro = minimapNode.GetComponentInChildren<TextMeshPro>();
        minimapNodeSpriteRenderer = minimapNode.GetComponent<SpriteRenderer>();

        musicController = FindAnyObjectByType<MusicController>
();    }

    public void ShowCorrectValue()
    {
        textMeshPro.text = correctValue.ToString();
    }

    public void HideValue()
    {
        textMeshPro.text = "";
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

        if (DifficultyController.showHintColorWhenDelivered)
        {
            musicController.PlayCorrectDeliveredSFX();
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

        if (DifficultyController.showHintUIPackageAndMinimap)
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

    public IEnumerator WrongDeliveryColorCoroutine()
    {
        musicController.PlayWrongDeliveredSFX();

        Color32 defaultMailboxColor = spriteRenderer.color;
        Color32 defaultMinimapNodeColor = minimapNodeSpriteRenderer.color;

        spriteRenderer.color = wrongDeliverdColor;
        minimapNodeSpriteRenderer.color = wrongDeliverdColor;

        yield return new WaitForSeconds(wrongDeliveryDelay);

        spriteRenderer.color = defaultMailboxColor;
        minimapNodeSpriteRenderer.color = defaultMinimapNodeColor;
    }

    public IEnumerator ShowHintValueCoroutine()
    {
        ShowCorrectValue();
        minimapNodeTextMeshPro.text = correctValue.ToString();
        yield return new WaitForSeconds(wrongDeliveryDelay);
        HideValue();
        minimapNodeTextMeshPro.text = "";
    }
}
