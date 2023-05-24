using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;

public class SelectLevelController : MonoBehaviour
{
    private static string PLAYER_PREFS_HIGHSCORE_TABLE = "HighscoreTable_";

    [SerializeField] public GameObject firstSelectedButton;
    [SerializeField] TextMeshProUGUI levelNameText;
    [SerializeField] TextMeshProUGUI informationText;
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

        highscoreTitleText.enabled = false;
        highscoreValueText.text = string.Empty;
        highscoreNameText.text = string.Empty;
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
        if (!PlayerPrefs.HasKey(PLAYER_PREFS_HIGHSCORE_TABLE + currentSelectedLevelEnum))
        {
            highscoreTitleText.enabled = false;
            highscoreValueText.text = string.Empty;
            highscoreNameText.text = string.Empty;
            return;
        }

        string jsonString = PlayerPrefs.GetString(PLAYER_PREFS_HIGHSCORE_TABLE + currentSelectedLevelEnum);
        Highscores loadedAllHighscores = JsonUtility.FromJson<Highscores>(jsonString);
        Debug.Log("loadedAllHighscores entries " + loadedAllHighscores.highscoreEntryList.Count);

        // Filter and order list for levelName
        List<HighscoreEntry> filterAndSortListForLevelName = loadedAllHighscores.highscoreEntryList
            .OrderByDescending(e => e.score)
            .Take(1)
            .ToList();

        highscoreTitleText.enabled = true;
        highscoreValueText.text = filterAndSortListForLevelName.First().score.ToString();
        highscoreNameText.text = filterAndSortListForLevelName.First().playerName;
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
