using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeliveryController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public GameObject collectedPackageOnCarSprite;
    [SerializeField] public Transform dropPackageLocation;
    [SerializeField] public float wrongDeliveryDelay = 2.0f;

    private DifficultyController difficultyController;

    public bool isCollidingWithMailbox;
    public bool isCollidingWithPackage;

    public Package currentCollectedPackage;
    private Package[] allPackages;
    public Mailbox currentCollidingMailbox;
    public Package currentCollidingPackage;

    private void Awake()
    {
        allPackages = FindObjectsOfType<Package>();
        difficultyController = FindObjectOfType<DifficultyController>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Mailbox"))
        {
            isCollidingWithMailbox = true;
            currentCollidingMailbox = other.GetComponent<Mailbox>();
        }

        if (other.tag.Equals("Package"))
        {
            isCollidingWithPackage = true;
            currentCollidingPackage = other.GetComponent<Package>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Mailbox"))
        {
            isCollidingWithMailbox = false;
            currentCollidingMailbox = null;
        }
        if (other.tag.Equals("Package"))
        {
            isCollidingWithPackage = false;
            currentCollidingPackage = null;
        }
    }

    public bool AllPackagesDelivered()
    {
        foreach (Package package in allPackages)
        {
            if (!package.isCorrectDelivered)
            {
                return false;
            }
        }
        return true;
    }

    public void DropOrPickupPackage()
    {
        if(currentCollidingPackage == null && currentCollidingMailbox == null && currentCollectedPackage == null)
        {
            return;
        }

        if (currentCollectedPackage != null && !isCollidingWithMailbox)
        {
            DropPackage();
        }
        else if (currentCollectedPackage == null && isCollidingWithPackage)
        {
            PickupPackage();
        }
        else if (currentCollectedPackage == null && !currentCollidingMailbox.hasReceivedCorrectPackage && isCollidingWithMailbox && !isCollidingWithPackage)
        {
            PickupDeliveredPackage();
        }
        else if (currentCollectedPackage != null && isCollidingWithMailbox)
        {
            TryToDeliverPackage();
        }
    }

    private void PickupPackage()
    {
        Debug.Log("Pickup Package");

        currentCollectedPackage = currentCollidingPackage;
        TextMeshPro packageTMPro = currentCollectedPackage.GetComponentInChildren<TextMeshPro>();

        collectedPackageOnCarSprite.SetActive(true);
        collectedPackageOnCarSprite.GetComponentInChildren<TextMeshPro>().text = packageTMPro.text;

        currentCollectedPackage.Pickedup();
    }

    private void DropPackage()
    {
        Debug.Log("Drop package");

        Vector2 dropLocation = new Vector2(dropPackageLocation.position.x, dropPackageLocation.position.y);
        currentCollectedPackage.Drop(dropLocation);

        collectedPackageOnCarSprite.SetActive(false);
        collectedPackageOnCarSprite.GetComponentInChildren<TextMeshPro>().text = "";

        currentCollectedPackage = null;
    }

    private void TryToDeliverPackage()
    {
        Debug.Log("Try to deliver package");

        int packageValue = currentCollectedPackage.Value();

        if (currentCollidingMailbox.hasReceivedPackage)
        {
            return;
        }

        if (currentCollidingMailbox.correctValue.Equals(packageValue))
        {
            Debug.Log("Package Correct Delivered");

            collectedPackageOnCarSprite.SetActive(false);
            collectedPackageOnCarSprite.GetComponentInChildren<TextMeshPro>().text = "";

            currentCollidingMailbox.CorrectPackageReceived();
            currentCollectedPackage.CorrectDelivered();

            currentCollectedPackage = null;
        }
        else if (!currentCollidingMailbox.hasReceivedCorrectPackage)
        {
            Debug.Log("Package Wrong Delivered");
            difficultyController.IncreaseWrongDelivery();

            if (difficultyController.canPackageBeDeliveredAtWrongNode)
            {
                collectedPackageOnCarSprite.SetActive(false);
                collectedPackageOnCarSprite.GetComponentInChildren<TextMeshPro>().text = "";

                currentCollidingMailbox.WrongPackageReceived(currentCollectedPackage.Value());
                currentCollectedPackage.WrongDelivered();

                currentCollectedPackage = null;
            }
            else
            {             
                if (difficultyController.showHintValueWhenWrongDelivered)
                {
                    StartCoroutine(currentCollidingMailbox.ShowHintValueCoroutine());
                }

                if (difficultyController.showHintColorWhenDelivered)
                {                    
                    StartCoroutine(currentCollidingMailbox.WrongDeliveryColorCoroutine());
                }
            }            
        }
    }

    private void PickupDeliveredPackage()
    {
        Debug.Log("Pickup delivered package");
        Package package = FindPackage(currentCollidingMailbox.receivedPackageValue);
        package.gameObject.SetActive(true);

        currentCollectedPackage = package;

        TextMeshPro packageTMPro = currentCollectedPackage.GetComponentInChildren<TextMeshPro>();

        collectedPackageOnCarSprite.SetActive(true);
        collectedPackageOnCarSprite.GetComponentInChildren<TextMeshPro>().text = packageTMPro.text;

        currentCollidingMailbox.PickupPackage();

        currentCollectedPackage.Pickedup();
    }

    private Package FindPackage(int value)
    {
        for (int i = 0; i < allPackages.Length; i++)
        {
            Package package = allPackages[i];
            if (package.Value().Equals(value))
            {
                return package;
            }
        }
        return null;
    }


}