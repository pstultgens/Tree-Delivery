using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] public LevelEnum level;
    [SerializeField] public GameObject lockBackgroundGameObject;
    [SerializeField] public GameObject levelFinishedCheckmarkGameObject;
    [SerializeField] Image lockImage;

    private Image lockBackgroundImage;
    private Color32 normalLockBackgroundColor;
    private Color32 selectedLockBackgroundColor = new Color32(94, 94, 94, 255);

    private Color32 normalLockImageColor;
    private Color32 selectedLockImageColor = new Color32(152, 152, 152, 255);
   
    private bool isLevelUnLocked;

    private EventSystem eventSystem;
    private SceneManager sceneManager;
    private Button button;

    private void Start()
    {
        eventSystem = EventSystem.current;
        sceneManager = FindObjectOfType<SceneManager>();
        button = GetComponent<Button>();

        isLevelUnLocked = PlayerPrefsRepository.Instance.IsLevelUnlocked(level);

        if (!isLevelUnLocked)
        {
            lockBackgroundImage = lockBackgroundGameObject.GetComponent<Image>();
            normalLockBackgroundColor = lockBackgroundImage.color;

            
            normalLockImageColor = lockImage.color;
            
        }

        if (lockBackgroundGameObject != null && isLevelUnLocked)
        {
            Debug.Log(level.GetName() + ": Unlocked");
            lockBackgroundGameObject.SetActive(false);
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
            levelFinishedCheckmarkGameObject.SetActive(true);
        }
        else
        {
            Debug.Log(level.GetName() + ": Unfinished");
            levelFinishedCheckmarkGameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isLevelUnLocked)
        {
            return;
        }

        if (IsButtonSelected() && !isLevelUnLocked)
        {
            lockBackgroundImage.color = selectedLockBackgroundColor;
            lockImage.color = selectedLockImageColor;

        }
        else if (!IsButtonSelected() && !isLevelUnLocked)
        {
            lockBackgroundImage.color = normalLockBackgroundColor;
            lockImage.color = normalLockImageColor;
        }
    }

    private bool IsButtonSelected()
    {
        return gameObject.Equals(eventSystem.currentSelectedGameObject);
    }
}
