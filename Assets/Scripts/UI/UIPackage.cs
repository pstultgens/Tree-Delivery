using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPackage : MonoBehaviour
{
    [SerializeField] GameObject imageGameObject;

    public void Delivered()
    {
        imageGameObject.SetActive(true);
    }

    public int Value()
    {
        TextMeshProUGUI tmPro = GetComponentInChildren<TextMeshProUGUI>();
        return int.Parse(tmPro.text);
    }
}
