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

    private Package[] allPackages;

    private void Awake()
    {
        allPackages = FindObjectsOfType<Package>();
    }

    void Start()
    {    
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
        foreach (Package package in allPackages)
        {
            if (!package.isDelivered)
            {
                return false;
            }            
        }
        return true;
    }
}
