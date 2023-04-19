using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelController : MonoBehaviour
{
    [SerializeField] public LevelLoader levelLoader;
    [SerializeField] public UIController uiController;

    [Header("Level Setup")]
    [SerializeField] public bool randomizePackageValues;
    [SerializeField] public int rangeStart = 1;
    [SerializeField] public int rangeEnd = 100;
    [SerializeField] public List<int> defaultPackageValuesLowToHigh; 

    [Header("Add Nodes in BST sorted order")]
    [SerializeField] public List<GameObject> nodes = new List<GameObject>();

    private DeliveryController deliveryController;

    private void Awake()
    {
        deliveryController = GameObject.FindGameObjectWithTag("Player").GetComponent<DeliveryController>();
    }

    void Start()
    {    
        if (randomizePackageValues)
        {
            UpdateNodes(GenerateUniqueRandomIntegersBasedOnNodes());
        }
        else
        {
            UpdateNodes(defaultPackageValuesLowToHigh);
        }
    }

    void Update()
    {
        if (deliveryController.AllPackagesDelivered())
        {
            LevelComplete();
        }
    }

    private void UpdateNodes(List<int> values)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            GameObject node = nodes[i];
            Package package = node.GetComponentInChildren<Package>();
            Mailbox mailbox = node.GetComponentInChildren<Mailbox>();

            int value = values[i];

            TextMeshPro packageTMPro = package.GetComponentInChildren<TextMeshPro>();

            packageTMPro.text = value.ToString();
            mailbox.correctValue = value;
        }

        uiController.ShowUIPackages(values);
    }

    private List<int> GenerateUniqueRandomIntegersBasedOnNodes()
    {
        int count = nodes.Count;      

        List<int> uniqueRandomIntegers = new List<int>();
        HashSet<int> usedIntegers = new HashSet<int>();
        System.Random random = new System.Random();

        while (uniqueRandomIntegers.Count < count)
        {
            int randomInteger = random.Next(rangeStart, rangeEnd + 1);

            if (!usedIntegers.Contains(randomInteger))
            {
                usedIntegers.Add(randomInteger);
                uniqueRandomIntegers.Add(randomInteger);
            }
        }

        // Sort the values from low to hight value
        uniqueRandomIntegers.Sort((a, b) => a.CompareTo(b));

        return uniqueRandomIntegers;
    }    

    

    private void LevelComplete()
    {
        
            levelLoader.LoadNextLevel();
        
    }

    
}
