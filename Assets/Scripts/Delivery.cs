using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Delivery : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] GameObject collectedPackage;
    [SerializeField] float destroyDelay = 0.5f;
    [SerializeField] float wrongDeliveryDelay = 2.0f;

    [SerializeField] Color32 correctDeliverdColor = new Color32(1, 1, 1, 1);
    [SerializeField] Color32 wrongDeliverdColor = new Color32(1, 1, 1, 1);


    private bool hasPackage;
    private int currentPackageValue;

    private TextMeshPro textMeshPro;


    private void Start()
    {
        textMeshPro = GetComponentInChildren<TextMeshPro>();
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TextMeshPro otherTMPro = other.GetComponentInChildren<TextMeshPro>();


        if (other.tag.Equals("Package") && !hasPackage)
        {
            Debug.Log("Package picked up");
            hasPackage = true;
            collectedPackage.SetActive(true);

            currentPackageValue = int.Parse(otherTMPro.text);
            textMeshPro.text = otherTMPro.text;

            Destroy(other.gameObject, destroyDelay);
        }

        if (other.tag.Equals("Mailbox") && hasPackage)
        {


            Mailbox mailbox = other.GetComponent<Mailbox>();
            SpriteRenderer mailboxSpriteRenderer = other.GetComponent<SpriteRenderer>();

            if (mailbox.correctValue.Equals(currentPackageValue))
            {
                Debug.Log("Package Correct Delivered");
                otherTMPro.text = currentPackageValue.ToString();
                hasPackage = false;
                collectedPackage.SetActive(false);

                UpdateMinimap(mailbox);

                // Remove obstacle and open path to next Node
                mailbox.OpenToNextNode();

                mailboxSpriteRenderer.color = correctDeliverdColor;

                textMeshPro.text = "";
            }
            else
            {
                Debug.Log("Package Wrong Delivered");
                SpriteRenderer minimapNodeSpriteRenderer = mailbox.minimapNode.GetComponent<SpriteRenderer>();
                TextMeshPro minimapNodeTMPro = mailbox.minimapNode.GetComponentInChildren<TextMeshPro>();

                StartCoroutine(GiveHintCoroutine(otherTMPro, minimapNodeTMPro, mailbox.correctValue.ToString()));
                StartCoroutine(WrongDeliveryColorCoroutine(mailboxSpriteRenderer, minimapNodeSpriteRenderer));
            }
        }
    }

    private void UpdateMinimap(Mailbox mailbox)
    {
        // Update Minimap Node
        mailbox.minimapNode.GetComponent<SpriteRenderer>().color = correctDeliverdColor;
        mailbox.minimapNode.GetComponentInChildren<TextMeshPro>().text = currentPackageValue.ToString();

        // Update Minimap Edges
        if (mailbox.minimapEdgeLeft != null)
        {
            mailbox.minimapEdgeLeft.GetComponent<SpriteRenderer>().color = correctDeliverdColor;
        }

        if (mailbox.minimapEdgeRight != null)
        {
            mailbox.minimapEdgeRight.GetComponent<SpriteRenderer>().color = correctDeliverdColor;
        }
    }

    IEnumerator WrongDeliveryColorCoroutine(SpriteRenderer spriteRenderer, SpriteRenderer minimapNodeSpriteRenderer)
    {
        Color32 defaultMailboxColor = spriteRenderer.color;
        Color32 defaultMinimapNodeColor = minimapNodeSpriteRenderer.color;

        spriteRenderer.color = wrongDeliverdColor;
        minimapNodeSpriteRenderer.color = wrongDeliverdColor;

        yield return new WaitForSeconds(wrongDeliveryDelay);

        spriteRenderer.color = defaultMailboxColor;
        minimapNodeSpriteRenderer.color = defaultMinimapNodeColor;
    }

    IEnumerator GiveHintCoroutine(TextMeshPro mailboxTMPro, TextMeshPro minimapNodeTMPro, string correctValue)
    {
        mailboxTMPro.text = correctValue;
        minimapNodeTMPro.text = correctValue;
        yield return new WaitForSeconds(wrongDeliveryDelay);
        mailboxTMPro.text = "";
        minimapNodeTMPro.text = "";
    }
}
