using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelector : MonoBehaviour
{

    [SerializeField] public LevelEnum level;
    [SerializeField] public GameObject lockImage;
    [SerializeField] public GameObject levelFinishedCheckmark;

    private SceneManager sceneManager;
    private Button button;

    private void Start()
    {
        sceneManager = FindObjectOfType<SceneManager>();
        button = GetComponent<Button>();

        if (lockImage != null && PlayerPrefsRepository.Instance.IsLevelUnlocked(level))
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

        if (PlayerPrefsRepository.Instance.IsLevelFinished(level))
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
}
