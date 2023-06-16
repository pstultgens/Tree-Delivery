using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;

public class DeliveryController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] GameObject collectedPackageOnCarSprite;
    [SerializeField] Transform dropPackageLocation;
    [SerializeField] float packagePickupDelay = 1f;


    private MMFeedbacks cannotRemoveParentFeedback;
    private MMFeedbacks cannotAddChildFeedback;

    public bool isCollidingWithSpot;
    public bool isCollidingWithPackage;

    public Package currentCollectedPackage;
    public Spot currentCollidingSpot;
    public Package currentCollidingPackage;

    private CarVfxHandler carVfxHandler;
    private Package[] allPackages;
    private bool canPackageBePickedUp = true;
    private bool isSwappingPackage = false;
    private Package previousDroppedPackage;

    private ScoreController scoreController;

    private void Awake()
    {
        carVfxHandler = GetComponent<CarVfxHandler>();

        allPackages = FindObjectsOfType<Package>();
        scoreController = FindObjectOfType<ScoreController>();

        GameObject cannotRemoveParentFeedbackGameObject = GameObject.FindGameObjectWithTag("CannotRemoveParentFeedback");
        if (cannotRemoveParentFeedbackGameObject != null)
        {
            cannotRemoveParentFeedback = cannotRemoveParentFeedbackGameObject.GetComponent<MMFeedbacks>();
        }

        GameObject cannotAddChildFeedbackGameObject = GameObject.FindGameObjectWithTag("CannotAddChildFeedback");
        if (cannotAddChildFeedbackGameObject != null)
        {
            cannotAddChildFeedback = cannotAddChildFeedbackGameObject.GetComponent<MMFeedbacks>();
        }
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

            if (currentCollectedPackage != null && !currentCollidingSpot.hasReceivedPackage)
            {
                if (currentCollidingSpot.CanDeliverPackage())
                {
                    DeliverPackage();
                }
                else
                {
                    CannotDeliverHasNoParent();
                }
            }
        }

        if (other.tag.Equals("Package"))
        {
            if (isSwappingPackage) { return; }

            isCollidingWithPackage = true;
            currentCollidingPackage = other.GetComponent<Package>();

            if (canPackageBePickedUp && currentCollectedPackage == null)
            {
                Debug.Log("Pickup Package, after pickup delay");
                PickupPackage();
            }
            else if (currentCollectedPackage == null && !currentCollidingPackage.Equals(previousDroppedPackage))
            {
                // Want to pickup an other package as previous, ignoring pickup delay
                Debug.Log("Pickup different Package as previous, ignore pickup delay");
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

    public bool AllPackagesCorrectDelivered()
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

    public bool AllPackagesDelivered()
    {
        foreach (Package package in allPackages)
        {
            if (!package.isDelivered)
            {
                return false;
            }
        }
        return true;
    }

    public void PlayerInputDropHandler()
    {
        if (currentCollidingPackage == null && currentCollidingSpot == null && currentCollectedPackage == null)
        {
            return;
        }

        if (currentCollidingPackage != null && currentCollectedPackage != null)
        {
            SwapPickupPackage();
            return;
        }

        if (currentCollidingPackage != null && currentCollectedPackage == null)
        {
            PickupPackage();
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
                if (currentCollidingSpot.CanRemovePackage())
                {
                    PickupDeliveredPackage();
                }
                else
                {
                    CannotRemoveHasChild();
                }
            }
            else if (!currentCollidingSpot.hasReceivedCorrectPackage)
            {
                if (currentCollidingSpot.CanRemovePackage())
                {
                    PickupDeliveredPackage();
                }
                else
                {
                    CannotRemoveHasChild();
                }
            }
        }
        else if (currentCollectedPackage != null
            && isCollidingWithSpot
            && currentCollidingSpot.hasReceivedPackage
            && HintController.Instance.canPackageBeDeliveredAtWrongNode)
        {
            SwapDeliveredPackage();
        }
        else if (
          currentCollectedPackage != null
          && isCollidingWithSpot
          && !currentCollidingSpot.hasReceivedPackage)
        {
            if (currentCollidingSpot.CanDeliverPackage())
            {
                DeliverPackage();
            }
            else
            {
                CannotDeliverHasNoParent();
            }
        }
    }

    private void CannotRemoveHasChild()
    {
        cannotRemoveParentFeedback.PlayFeedbacks();
        scoreController.IncreaseCannotRemoveHasChildCounter();
        currentCollidingSpot.ShowHintCannotRemovePackage();
    }

    private void CannotDeliverHasNoParent()
    {
        cannotAddChildFeedback.PlayFeedbacks();
        scoreController.IncreaseCannotDeliverHasNoParentCounter();
        currentCollidingSpot.ShowHintCannotDeliverPackage();
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
        isSwappingPackage = false;
    }

    private void DropPackage()
    {
        Debug.Log("Drop package");
        previousDroppedPackage = currentCollectedPackage;

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

            currentCollectedPackage = null;
        }
        else if (!currentCollidingSpot.hasReceivedCorrectPackage)
        {
            Debug.Log("Package Wrong Delivered");

            currentCollectedPackage.AddCounterWrongValueDelivered();

            scoreController.IncreaseWrongValueDeliveredCounter();

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

    private void SwapPickupPackage()
    {
        Debug.Log("Swap with other Package to pickup");
        isSwappingPackage = true;
        DropPackage();
        PickupPackage();
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

        }
        else if (!currentCollidingSpot.hasReceivedCorrectPackage)
        {
            Debug.Log("Swap Package Wrong Delivered");

            carPackage.AddCounterWrongValueDelivered();

            scoreController.IncreaseWrongValueDeliveredCounter();

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