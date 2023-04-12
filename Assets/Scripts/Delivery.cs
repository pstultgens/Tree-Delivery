using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Delivery : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] GameObject collectedPackage;
    [SerializeField] float destroyDelay = 0.5f;
    [SerializeField] float wrongDeliveryColorDelay = 2.0f;

    [SerializeField] Color32 correctDeliverdColor = new Color32(1, 1, 1, 1);
    [SerializeField] Color32 wrongDeliverdColor = new Color32(1, 1, 1, 1);


    private bool hasPackage;
    private int currentPackageValue;

    private TextMeshPro textMeshPro;
    private SpriteRenderer spriteRenderer;


    private void Start()
    {
        textMeshPro = GetComponentInChildren<TextMeshPro>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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


                // Update Minimap Node
                mailbox.minimapNode.GetComponent<SpriteRenderer>().color = correctDeliverdColor;
                mailbox.minimapNode.GetComponentInChildren<TextMeshPro>().text = currentPackageValue.ToString();

                mailboxSpriteRenderer.color = correctDeliverdColor;

                textMeshPro.text = "";
            }
            else
            {
                Debug.Log("Package Wrong Delivered");

                StartCoroutine(WrongDeliveryColor(mailboxSpriteRenderer));
            }

        }
    }

    IEnumerator WrongDeliveryColor(SpriteRenderer spriteRenderer)
    {
        Color32 defaultMailboxColor = spriteRenderer.color;
        spriteRenderer.color = wrongDeliverdColor;
        yield return new WaitForSeconds(wrongDeliveryColorDelay);
        spriteRenderer.color = defaultMailboxColor;
    }

    


}
