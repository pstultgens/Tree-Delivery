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

    public bool allPackagesCorrectDelivered;
    public bool isCollidingWithMailbox;
    public bool isCollidingWithPackage;

    private CarVfxHandler carVfxHandler;

    public Package currentCollectedPackage;
    private Package[] allPackages;
    public Mailbox currentCollidingMailbox;
    public Package currentCollidingPackage;

    private ScoreController scoreController;

    private void Awake()
    {
        allPackages = FindObjectsOfType<Package>();
        carVfxHandler = GetComponent<CarVfxHandler>();
        scoreController = FindObjectOfType<ScoreController>();
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

    private bool AllPackagesCorrectDelivered()
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
        if (currentCollidingPackage == null && currentCollidingMailbox == null && currentCollectedPackage == null)
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
        else if (currentCollectedPackage == null &&
            !currentCollidingMailbox.hasReceivedCorrectPackage &&
            currentCollidingMailbox.hasReceivedPackage &&
            isCollidingWithMailbox &&
            !isCollidingWithPackage)
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

        carVfxHandler.PlayPickupPackageVFX(dropPackageLocation.position);
    }

    private void DropPackage()
    {
        Debug.Log("Drop package");

        Vector2 dropLocation = new Vector2(dropPackageLocation.position.x, dropPackageLocation.position.y);
        currentCollectedPackage.Drop(dropLocation);

        collectedPackageOnCarSprite.SetActive(false);
        collectedPackageOnCarSprite.GetComponentInChildren<TextMeshPro>().text = "";

        carVfxHandler.PlayDropPackageVFX(dropLocation);

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
            currentCollectedPackage.CorrectDelivered(currentCollidingMailbox);

            allPackagesCorrectDelivered = AllPackagesCorrectDelivered();
            currentCollectedPackage = null;
        }
        else if (!currentCollidingMailbox.hasReceivedCorrectPackage)
        {
            Debug.Log("Package Wrong Delivered");

            currentCollectedPackage.AddCounterWrongDelivered();

            if (scoreController != null)
            {
                scoreController.DecreaseScorePackageWrongDelivered(currentCollidingMailbox);
            }

            DifficultyController.Instance.IncreaseWrongDelivery();

            if (DifficultyController.Instance.canPackageBeDeliveredAtWrongNode)
            {
                collectedPackageOnCarSprite.SetActive(false);
                collectedPackageOnCarSprite.GetComponentInChildren<TextMeshPro>().text = "";

                currentCollidingMailbox.WrongPackageReceived(currentCollectedPackage.Value());
                currentCollectedPackage.WrongDelivered();

                currentCollectedPackage = null;
            }
            else
            {
                if (DifficultyController.Instance.showHintValueWhenWrongDelivered)
                {
                    currentCollidingMailbox.ShowWrongDeliveryHintValue();
                }

                if (DifficultyController.Instance.showHintColorWhenDelivered)
                {
                    currentCollidingMailbox.ShowWrongDeliveryHintColor();
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