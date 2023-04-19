using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] public UIPackage uiPackagePrefab;
    [SerializeField] public GameObject uiPackageWindowGrid;

    private List<UIPackage> allUIPackages = new List<UIPackage>();

    public void ShowUIPackages(List<int> packageValues)
    {
        Debug.Log("Show the UI packages");
        for (int i = 0; i < packageValues.Count; i++)
        {
            AddPackageToUI(packageValues[i]);
        }
    }

    public void HideUIPackages()
    {
        Debug.Log("Hide the UI packages");
        uiPackageWindowGrid.transform.parent.gameObject.SetActive(false);
    }

    private void AddPackageToUI(int value)
    {
        TextMeshProUGUI uiPackageTMPro = uiPackagePrefab.GetComponentInChildren<TextMeshProUGUI>();
        uiPackageTMPro.text = value.ToString();

        GameObject instantiatedUIPackage = Instantiate(uiPackagePrefab.gameObject, uiPackageWindowGrid.transform);

        instantiatedUIPackage.transform.SetParent(uiPackageWindowGrid.transform, false);

        allUIPackages.Add(instantiatedUIPackage.GetComponent<UIPackage>());
    }

    public void PackageDelivered(int value)
    {
        foreach (UIPackage uiPackage in allUIPackages)
        {
            if (uiPackage.Value() == value)
            {
                uiPackage.Delivered();
                return;
            }
        }
    }
}