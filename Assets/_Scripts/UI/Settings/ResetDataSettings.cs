using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDataSettings : MonoBehaviour
{
    private SceneManager sceneManager;

    private void Start()
    {
        sceneManager = FindObjectOfType<SceneManager>();
    }

    public void ResetData()
    {
        PlayerPrefsRepository.Instance.ResetAllLevels();
        PlayerPrefsRepository.Instance.ResetHighscores();       

        sceneManager.BackToMainMenu();
    } 
}