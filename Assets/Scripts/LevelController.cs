using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] public LevelLoader levelLoader;

    [Header("Level Setup")]
    [SerializeField] public bool randomizePackageValues;
    [SerializeField] public int[] defaultPackageValues;
    [SerializeField] public List<GameObject> nodes = new List<GameObject>();

    private List<Package> packages = new List<Package>();

    void Start()
    {
        DeterminePackagesFromNodes();


        if (randomizePackageValues)
        {
            GenerateRandomPackageValues();
        }
        else
        {
            UseDefaultPackageValues();
        }
    }

    void Update()
    {
        if (AllPackagesDelivered())
        {
            LevelComplete();
        }
    }

    private void DeterminePackagesFromNodes()
    {
        foreach (GameObject gameObject in nodes)
        {
            packages.Add(gameObject.GetComponentInChildren<Package>());
        }
    }


    private void GenerateRandomPackageValues()
    {
        
    }

    private void UseDefaultPackageValues()
    {
        
    }

    private void LevelComplete()
    {
        if (AllPackagesDelivered())
        {
            levelLoader.LoadNextLevel();
        }
    }

    private bool AllPackagesDelivered()
    {
        foreach (Package package in packages)
        {
            if (!package.isDelivered)
            {
                return false;
            }            
        }
        return true;
    }
}
