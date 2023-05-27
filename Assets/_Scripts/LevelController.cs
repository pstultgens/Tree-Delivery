using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelController : MonoBehaviour
{
    [SerializeField] public UIController uiController;

    [Header("Level Setup")]
    [SerializeField] public int rangeStart = 1;
    [SerializeField] public int rangeEnd = 100;
    [SerializeField] public List<int> defaultPackageValuesLowToHigh;

    [Header("Add Nodes in BST sorted order")]
    [SerializeField] public List<GameObject> nodes = new List<GameObject>();

    private bool isInLevelCompleteTransition = false;
    private bool alreadyAllPackagesDelivered = false;

    private DeliveryController deliveryController;
    private SceneManager sceneManager;

    void Start()
    {
        deliveryController = FindObjectOfType<DeliveryController>();
        sceneManager = FindObjectOfType<SceneManager>();

        if (HintController.Instance.randomizePackageValues)
        {
            FillNodes(GenerateUniqueRandomIntegersBasedOnNodes());
        }
        else
        {
            FillNodes(defaultPackageValuesLowToHigh);
        }

        uiController.ShowLevelNameWindow();
    }

    void Update()
    {
        if (deliveryController.AllPackagesCorrectDelivered())
        {
            // This should only be once
            if (isInLevelCompleteTransition)
            {
                return;
            }
            isInLevelCompleteTransition = true;

            // Color all nodes green
            foreach (GameObject node in nodes)
            {
                Spot spot = node.GetComponentInChildren<Spot>();
                spot.ShowHintColor();
            }

            StartCoroutine(LevelCompleteCoroutine());
        }
        else if (deliveryController.AllPackagesDelivered())
        {
            if (alreadyAllPackagesDelivered)
            {
                return;
            }

            alreadyAllPackagesDelivered = true;
            Debug.Log("All packages are delivered, but not all are correct delivered!");
            // Show some text

            // All Spots show hint color
            foreach (GameObject node in nodes)
            {
                Spot spot = node.GetComponentInChildren<Spot>();
                spot.ShowHintColor();
                HintController.Instance.showHintColorWhenDelivered = true;
            }
        }
    }

    private IEnumerator LevelCompleteCoroutine()
    {
        SceneManager.isGamePaused = true;

        UpdateLevelData();

        // Wait some time before showing the Level Complete Window
        yield return new WaitForSeconds(1.5f);

        if (LevelEnum.Tutorial.Equals(HintController.Instance.currentLevel))
        {
            sceneManager.GoToScene("Select Level Menu");
        }
        else if (LevelEnum.Test.Equals(HintController.Instance.currentLevel))
        {
            sceneManager.ShowTestLevelComplete();
        }
        else
        {
            sceneManager.ShowLevelComplete();
        }
    }

    private void UpdateLevelData()
    {
        PlayerPrefsRepository.Instance.UnlockNextLevel();
        PlayerPrefsRepository.Instance.UpdateFinishedLevel();
    }

    private void FillNodes(List<int> values)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            GameObject node = nodes[i];
            Package package = node.GetComponentInChildren<Package>();
            Spot spot = node.GetComponentInChildren<Spot>();

            int value = values[i];

            TextMeshPro packageTMPro = package.GetComponentInChildren<TextMeshPro>();

            packageTMPro.text = value.ToString();
            spot.correctValue = value;

            if (HintController.Instance.showAlreadyCorrectValueOnNode)
            {
                spot.ShowHintCorrectValue();
            }
        }
        if (HintController.Instance.showUIPackages)
        {
            if (HintController.Instance.randomizeOrderUIPackages)
            {
                values = RandomizeList(values);
            }
            uiController.ShowUIPackages(values);
        }
        else
        {
            uiController.HideUIPackages();
        }
    }

    private List<int> RandomizeList(List<int> values)
    {
        // Use Fisher-Yates shuffle algorithm
        for (int i = values.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = values[i];
            values[i] = values[randomIndex];
            values[randomIndex] = temp;
        }

        return values;
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

}
