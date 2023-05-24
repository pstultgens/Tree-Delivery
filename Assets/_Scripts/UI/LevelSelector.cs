using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelector : MonoBehaviour
{
    private static string PLAYER_PREFS_LEVEL_UNLOCKED = "LevelUnlocked_";

    [SerializeField] public LevelEnum level;
    [SerializeField] public GameObject lockImage;

    private SceneManager sceneManager;
    private Button button;

    private void Start()
    {
        sceneManager = FindObjectOfType<SceneManager>();
        button = GetComponent<Button>();

        if (lockImage != null && IsLevelUnlocked())
        {
            Debug.Log("Level unlocked");
            lockImage.SetActive(false);
            button.onClick.AddListener(() => sceneManager.LevelSelect(level));
        }
        else
        {
            Debug.Log("Level locked");
            //lockImage.SetActive(true);
            button.onClick.RemoveAllListeners();
        }
    }

    private bool IsLevelUnlocked()
    {
        if (PlayerPrefs.HasKey(PLAYER_PREFS_LEVEL_UNLOCKED + level))
        {
            return bool.Parse(PlayerPrefs.GetString(PLAYER_PREFS_LEVEL_UNLOCKED + level));
        }
        return false;
    }
}
