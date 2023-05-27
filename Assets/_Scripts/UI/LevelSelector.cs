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
            Debug.Log(level.GetName() + ": Unlocked");
            lockImage.SetActive(false);
            button.onClick.AddListener(() => sceneManager.LevelSelect(level));
        }
        else
        {
            Debug.Log(level.GetName() + ": Locked");
            button.onClick.RemoveAllListeners();
        }

        if (PlayerPrefsRepository.Instance.IsLevelFinished(level))
        {
            Debug.Log(level.GetName() + ": Finished");
            levelFinishedCheckmark.SetActive(true);
        }
        else
        {
            Debug.Log(level.GetName() + ": Unfinished");
            levelFinishedCheckmark.SetActive(false);
        }
    }
}
