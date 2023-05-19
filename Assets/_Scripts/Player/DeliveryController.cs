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
    [SerializeField] float packagePickupDelay = 1f;

    public bool allPackagesCorrectDelivered;
    public bool isCollidingWithSpot;
    public bool isCollidingWithPackage;      

    public Package currentCollectedPackage;    
    public Spot currentCollidingSpot;
    public Package currentCollidingPackage;

    private CarVfxHandler carVfxHandler;
    private Package[] allPackages;
    private ScoreController scoreController;
    private bool canPackageBePickedUp = true;
   

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

            if (canPackageBePickedUp && currentCollectedPackage == null)
            {
                PickupPackage();
            }
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
        else if (currentCollectedPackage == null &&
            isCollidingWithSpot &&
            !isCollidingWithPackage &&
            currentCollidingSpot.hasReceivedPackage)
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

            if (!currentCollidingSpot.hasReceivedPackage)
            {
                DeliverPackage();
            }
            else if (currentCollidingSpot.hasReceivedPackage && HintController.Instance.canPackageBeDeliveredAtWrongNode)
            {

                SwapDeliveredPackage();
            }
        }
    }

    private IEnumerator PackagePickupDelayCoroutine()
    {
        canPackageBePickedUp = false;      
        yield return new WaitForSeconds(packagePickupDelay);
        canPackageBePickedUp = true;
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

        StartCoroutine(PackagePickupDelayCoroutine());
    }

    private void DeliverPackage()
    {
        Debug.Log("Deliver package");

        int packageValue = currentCollectedPackage.Value();

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

    private void SwapDeliveredPackage()
    {
        Debug.Log("Swap Package with Spot");
        Package carPackage = currentCollectedPackage;

        PickupDeliveredPackage();

        int packageValue = carPackage.Value();

        if (currentCollidingSpot.correctValue.Equals(packageValue))
        {
            Debug.Log("Swap Package Correct Delivered");

            currentCollidingSpot.CorrectPackageReceived();
            carPackage.CorrectDelivered(currentCollidingSpot);

            allPackagesCorrectDelivered = AllPackagesCorrectDelivered();
        }
        else if (!currentCollidingSpot.hasReceivedCorrectPackage)
        {
            Debug.Log("Swap Package Wrong Delivered");

            carPackage.AddCounterWrongDelivered();

            HintController.Instance.IncreaseWrongDelivery();

            currentCollidingSpot.WrongPackageReceived(carPackage.Value());
            carPackage.WrongDelivered();
        }
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