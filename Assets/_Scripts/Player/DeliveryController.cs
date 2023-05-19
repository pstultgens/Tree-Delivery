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
    public bool isCollidingWithSpot;
    public bool isCollidingWithPackage;

    private CarVfxHandler carVfxHandler;

    public Package currentCollectedPackage;
    private Package[] allPackages;
    public Spot currentCollidingSpot;
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
        if (other.tag.Equals("Spot"))
        {
            isCollidingWithSpot = true;
            currentCollidingSpot = other.GetComponent<Spot>();
        }

        if (other.tag.Equals("Package"))
        {
            isCollidingWithPackage = true;
            currentCollidingPackage = other.GetComponent<Package>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Spot"))
        {
            isCollidingWithSpot = false;
            currentCollidingSpot = null;
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
        if (currentCollidingPackage == null && currentCollidingSpot == null && currentCollectedPackage == null)
        {
            return;
        }

        if (currentCollectedPackage != null && !isCollidingWithSpot)
        {
            DropPackage();
        }
        else if (currentCollectedPackage == null && isCollidingWithPackage)
        {
            PickupPackage();
        }
        else if (currentCollectedPackage == null &&
            currentCollidingSpot.hasReceivedPackage &&
            isCollidingWithSpot &&
            !isCollidingWithPackage)
        {
            if (HintController.Instance.canPackageBeDeliveredAtWrongNode)
            {
                PickupDeliveredPackage();
            }
            else if (!currentCollidingSpot.hasReceivedCorrectPackage)
            {
                PickupDeliveredPackage();
            }

        }
        else if (currentCollectedPackage != null && isCollidingWithSpot)
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

        if (currentCollidingSpot.hasReceivedPackage)
        {
            return;
        }

        if (currentCollidingSpot.correctValue.Equals(packageValue))
        {
            Debug.Log("Package Correct Delivered");

            collectedPackageOnCarSprite.SetActive(false);
            collectedPackageOnCarSprite.GetComponentInChildren<TextMeshPro>().text = "";

            currentCollidingSpot.CorrectPackageReceived();
            currentCollectedPackage.CorrectDelivered(currentCollidingSpot);

            allPackagesCorrectDelivered = AllPackagesCorrectDelivered();
            currentCollectedPackage = null;
        }
        else if (!currentCollidingSpot.hasReceivedCorrectPackage)
        {
            Debug.Log("Package Wrong Delivered");

            currentCollectedPackage.AddCounterWrongDelivered();

            HintController.Instance.IncreaseWrongDelivery();

            if (HintController.Instance.canPackageBeDeliveredAtWrongNode)
            {
                collectedPackageOnCarSprite.SetActive(false);
                collectedPackageOnCarSprite.GetComponentInChildren<TextMeshPro>().text = "";

                currentCollidingSpot.WrongPackageReceived(currentCollectedPackage.Value());
                currentCollectedPackage.WrongDelivered();

                currentCollectedPackage = null;
            }
            else
            {
                if (scoreController != null)
                {
                    scoreController.DecreaseScorePackageWrongDelivered(currentCollidingSpot);
                }

                if (HintController.Instance.showHintValueWhenWrongDelivered)
                {
                    currentCollidingSpot.ShowWrongDeliveryHintValue();
                }

                if (HintController.Instance.showHintColorWhenDelivered)
                {
                    currentCollidingSpot.ShowWrongDeliveryHintColor();
                }
            }
        }
    }

    private void PickupDeliveredPackage()
    {
        Debug.Log("Pickup delivered package");
        Package package = FindPackage(currentCollidingSpot.receivedPackageValue);
        package.gameObject.SetActive(true);

        currentCollectedPackage = package;

        TextMeshPro packageTMPro = currentCollectedPackage.GetComponentInChildren<TextMeshPro>();

        collectedPackageOnCarSprite.SetActive(true);
        collectedPackageOnCarSprite.GetComponentInChildren<TextMeshPro>().text = packageTMPro.text;

        currentCollidingSpot.PickupPackage();

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