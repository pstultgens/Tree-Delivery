using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelector : MonoBehaviour
{
    private static string PLAYER_PREFS_LEVEL = "Level_";

    [SerializeField] public LevelEnum level;
    [SerializeField] public GameObject lockImage;
    [SerializeField] public GameObject levelFinishedCheckmark;

    private SceneManager sceneManager;
    private Button button;

    private void Start()
    {
        sceneManager = FindObjectOfType<SceneManager>();
        button = GetComponent<Button>();

        if (lockImage != null && IsLevelUnlocked())
        {
            Debug.Log("Level unlocked: " + level);
            lockImage.SetActive(false);
            button.onClick.AddListener(() => sceneManager.LevelSelect(level));
        }
        else
        {
            Debug.Log("Level locked: " + level);
            button.onClick.RemoveAllListeners();
        }

        if (IsLevelFinished())
        {
            Debug.Log("Level finished: " + level);
            levelFinishedCheckmark.SetActive(true);
        }
        else
        {
            Debug.Log("Level unfinished: " + level);
            levelFinishedCheckmark.SetActive(false);
        }
    }

    private bool IsLevelUnlocked()
    {
        if (PlayerPrefs.HasKey(PLAYER_PREFS_LEVEL + level))
        {
            string jsonString = PlayerPrefs.GetString(PLAYER_PREFS_LEVEL + level);
            Level loadedLevel = JsonUtility.FromJson<Level>(jsonString);

            return loadedLevel.unlocked;
        }
        return false;
    }

    private bool IsLevelFinished()
    {
        if (PlayerPrefs.HasKey(PLAYER_PREFS_LEVEL + level))
        {
            string jsonString = PlayerPrefs.GetString(PLAYER_PREFS_LEVEL + level);
            Level loadedLevel = JsonUtility.FromJson<Level>(jsonString);

            return loadedLevel.finished;
        }
        return false;
    }
}
