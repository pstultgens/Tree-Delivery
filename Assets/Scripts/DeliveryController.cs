using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeliveryController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] GameObject collectedPackageOnCarSprite;
    [SerializeField] Transform dropPackageLocation;
    [SerializeField] float wrongDeliveryDelay = 2.0f;

    [SerializeField] Color32 correctDeliverdColor = new Color32(1, 1, 1, 1);
    [SerializeField] Color32 wrongDeliverdColor = new Color32(1, 1, 1, 1);
        
    private Package collectedPackage;
    private int collectedPackageValue;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TextMeshPro otherTMPro = other.GetComponentInChildren<TextMeshPro>();


        if (other.tag.Equals("Package") && collectedPackage == null)
        {
            Debug.Log("Package picked up");
            collectedPackage = other.GetComponent<Package>();

            collectedPackageOnCarSprite.SetActive(true);
            collectedPackageOnCarSprite.GetComponentInChildren<TextMeshPro>().text = otherTMPro.text;
            collectedPackageValue = int.Parse(otherTMPro.text);

            other.GetComponent<Package>().Pickedup();
        }

        if (other.tag.Equals("Mailbox") && collectedPackage != null)
        {


            Mailbox mailbox = other.GetComponent<Mailbox>();
            SpriteRenderer mailboxSpriteRenderer = other.GetComponent<SpriteRenderer>();

            if (mailbox.correctValue.Equals(collectedPackageValue))
            {
                Debug.Log("Package Correct Delivered");
                otherTMPro.text = collectedPackageValue.ToString();
                
                collectedPackageOnCarSprite.SetActive(false);
                collectedPackageOnCarSprite.GetComponentInChildren<TextMeshPro>().text = "";

                collectedPackage.Delivered();
                collectedPackage = null;

                UpdateMinimap(mailbox);

                // Remove obstacle and open path to next Node
                mailbox.OpenToNextNode();

                mailboxSpriteRenderer.color = correctDeliverdColor;

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

    public void DropPackage()
    {
        if (collectedPackage != null)
        {
            Debug.Log("Drop package");

            Vector2 dropLocation = new Vector2(dropPackageLocation.position.x, dropPackageLocation.position.y);
            collectedPackage.Drop(dropLocation);

            collectedPackageOnCarSprite.SetActive(false);
            collectedPackageOnCarSprite.GetComponentInChildren<TextMeshPro>().text = "";

            collectedPackage = null;
        }
    }

    private void UpdateMinimap(Mailbox mailbox)
    {
        // Update Minimap Node
        mailbox.minimapNode.GetComponent<SpriteRenderer>().color = correctDeliverdColor;
        mailbox.minimapNode.GetComponentInChildren<TextMeshPro>().text = collectedPackageValue.ToString();

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
