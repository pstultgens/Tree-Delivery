using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputHighscore : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;

    

    public string GetName()
    {
        return inputField.text;
    }

}

