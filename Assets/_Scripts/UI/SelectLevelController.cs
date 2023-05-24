using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;

public class SelectLevelController : MonoBehaviour
{
    [SerializeField] public GameObject firstSelectedButton;
    [SerializeField] TextMeshProUGUI levelNameText;
    [SerializeField] TextMeshProUGUI informationText;

    [SerializeField] GameObject highscoreGameObject;
    [SerializeField] TextMeshProUGUI highscoreTitleText;
    [SerializeField] TextMeshProUGUI highscoreValueText;
    [SerializeField] TextMeshProUGUI highscoreNameText;

    private GameObject currentSelectedLevel;
    private LevelEnum currentSelectedLevelEnum;
    private EventSystem eventSystem;

    private void Start()
    {
        eventSystem = EventSystem.current;
        SetFirstSelectedUIButton(firstSelectedButton);
    }

    private void Update()
    {
        if (ChangedLevelSelection())
        {
            currentSelectedLevel = eventSystem.currentSelectedGameObject;
            Debug.Log("Changed Level Selection to: " + currentSelectedLevel);

            LevelSelector levelSelector = currentSelectedLevel.GetComponent<LevelSelector>();
            if (levelSelector != null)
            {
                currentSelectedLevelEnum = levelSelector.level;
                levelNameText.text = currentSelectedLevelEnum.GetName();
                informationText.text = currentSelectedLevelEnum.GetInformation();
                GetHighscore();
            }
            else
            {
                ShowNoneInformation();
            }
        }
    }

    private void ShowNoneInformation()
    {
        levelNameText.text = string.Empty;
        informationText.text = string.Empty;

        highscoreGameObject.SetActive(false);
    }

    private bool ChangedLevelSelection()
    {
        if (currentSelectedLevel == null)
        {
            return false;
        }
        return !currentSelectedLevel.Equals(eventSystem.currentSelectedGameObject);
    }

    private void GetHighscore()
    {
        HighscoreEntry highscore = PlayerPrefsRepository.Instance.GetHighscore(currentSelectedLevelEnum);

        if (highscore == null)
        {
            highscoreGameObject.SetActive(false);
        }
        else
        {
            highscoreGameObject.SetActive(true);
            highscoreValueText.text = highscore.score.ToString();
            highscoreNameText.text = highscore.playerName;
        }
    }

    private void SetFirstSelectedUIButton(GameObject firstSelectedButton)
    {
        eventSystem.SetSelectedGameObject(firstSelectedButton, new BaseEventData(eventSystem));
        SimulateOnSelect(firstSelectedButton);
        currentSelectedLevel = firstSelectedButton;
        currentSelectedLevelEnum = currentSelectedLevel.GetComponent<LevelSelector>().level;
        GetHighscore();
    }

    private void SimulateOnSelect(GameObject selectedGameObject)
    {
        // Create a new pointer event
        PointerEventData pointerEvent = new PointerEventData(EventSystem.current);

        // Simulate an OnSelect event
        ExecuteEvents.Execute(selectedGameObject, pointerEvent, ExecuteEvents.selectHandler);
    }
}
