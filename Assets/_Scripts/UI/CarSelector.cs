using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CarSelector : MonoBehaviour
{
    [SerializeField] GameObject lockBackgroundGameObject;
    [SerializeField] Image lockImage;
    [SerializeField] TextMeshProUGUI lockText;

    private Image lockBackgroundImage;
    private Color32 normalLockBackgroundColor;
    private Color32 selectedLockBackgroundColor = new Color32(94, 94, 94, 255);

    private Color32 normalLockImageColor;
    private Color32 selectedLockImageColor = new Color32(152, 152, 152, 255);

    private Color32 normalLockTextColor;
    private Color32 selectedLockTextColor = new Color32(160, 160, 160, 255);

    private SceneManager sceneManager;
    private EventSystem eventSystem;
    private bool isCarUnlocked;
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        sceneManager = FindObjectOfType<SceneManager>();
        eventSystem = EventSystem.current;
        button = GetComponent<Button>();
        isCarUnlocked = PlayerPrefsRepository.Instance.AllLevelsFinished();

        if (!isCarUnlocked)
        {
            lockBackgroundImage = lockBackgroundGameObject.GetComponent<Image>();
            normalLockBackgroundColor = lockBackgroundImage.color;
            normalLockImageColor = lockImage.color;
            normalLockTextColor = lockText.color;
        }
        else
        {
            Debug.Log("New Car Unlocked");
            lockBackgroundGameObject.SetActive(false);
            button.onClick.AddListener(() => sceneManager.CarSelect("CarPurple"));
        }
    }


    private void Update()
    {
        if (isCarUnlocked)
        {
            return;
        }

        if (IsButtonSelected() && !isCarUnlocked)
        {
            lockBackgroundImage.color = selectedLockBackgroundColor;
            lockImage.color = selectedLockImageColor;
            lockText.color = selectedLockTextColor;

        }
        else if (!IsButtonSelected() && !isCarUnlocked)
        {
            lockBackgroundImage.color = normalLockBackgroundColor;
            lockImage.color = normalLockImageColor;
            lockText.color = normalLockTextColor;
        }
    }

    private bool IsButtonSelected()
    {
        return gameObject.Equals(eventSystem.currentSelectedGameObject);
    }
}
