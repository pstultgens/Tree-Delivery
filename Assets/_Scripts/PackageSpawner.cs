using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageSpawner : MonoBehaviour
{
    [SerializeField] Transform packageSpawnPositionStart;
    [SerializeField] Transform packageSpawnPositionEnd;
    [SerializeField] Animator animator;

    [SerializeField] float moveSpeed = 5f;

    private bool isMoving;

    private List<Package> packagesToSpawn = new List<Package>();
    private List<Package> spawnedPackages = new List<Package>();
    private Package spawnedPackage;

    void Start()
    {
        if (HintController.Instance.spawnPackageAfterPackage)
        {
            packagesToSpawn = new List<Package>(FindObjectsOfType<Package>());
            DisableAllPackages();
        }
    }

    void Update()
    {
        if (SceneManager.isGamePaused || SceneManager.isCountingDown)
        {
            return;
        }

        if (packagesToSpawn.Count > 0 && AllSpawnedPackagesDelivered())
        {
            SpawnPackage();
        }

        if (isMoving)
        {
            // Calculate the distance between the item's current position and the destination
            float distanceToDestination = Vector3.Distance(spawnedPackage.transform.position, packageSpawnPositionEnd.transform.position);

            // Move the item towards the destination using Lerp for smooth movement
            spawnedPackage.transform.position = Vector3.Lerp(spawnedPackage.transform.position, packageSpawnPositionEnd.transform.position, moveSpeed * Time.deltaTime / distanceToDestination);

            // Check if the item has reached close enough to the destination
            if (distanceToDestination < 0.01f)
            {
                // Stop moving and perform any desired actions upon reaching the destination
                isMoving = false;
                OnItemReachedDestination();
            }
        }
    }

    private void DisableAllPackages()
    {
        foreach (Package package in packagesToSpawn)
        {
            package.gameObject.SetActive(false);
        }
    }

    private void SpawnPackage()
    {
        int randomIndex = Random.Range(0, packagesToSpawn.Count);
        spawnedPackage = packagesToSpawn[randomIndex];
        packagesToSpawn.RemoveAt(randomIndex);
        spawnedPackage.transform.position = packageSpawnPositionStart.position;
        spawnedPackage.gameObject.SetActive(true);
        spawnedPackages.Add(spawnedPackage);
        MoveItemToDestination();
    }

    private bool AllSpawnedPackagesDelivered()
    {

        foreach (Package package in spawnedPackages)
        {
            if (!package.isDelivered)
            {
                return false;
            }
        }
        return true;
    }

    private void MoveItemToDestination()
    {
        isMoving = true;
        animator.SetBool("BounceStart", true);
    }

    private void OnItemReachedDestination()
    {
        animator.SetBool("BounceStart", false);
    }
}
