using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    [SerializeField] Sprite trophySprite;

    [SerializeField] Color32 trophy1Color = new Color32(255, 255, 255, 255); // F8F15A
    [SerializeField] Color32 trophy2Color = new Color32(255, 255, 255, 255); // 8E8E8E
    [SerializeField] Color32 trophy3Color = new Color32(255, 255, 255, 255); // BF762E

    [SerializeField] TextMeshProUGUI levelNameText;
    [SerializeField] Transform entryContainer;
    [SerializeField] Transform entryTemplate;
    [SerializeField] Color32 entryHighlightColor = new Color32(25, 239, 181, 255);

    private List<Transform> highscoreEntryTransformList;
    private bool highscoreEntryHighlighted;

    private ScoreController scoreController;

    private void Awake()
    {
        scoreController = FindObjectOfType<ScoreController>();
    }

    private void CleanHighscoreTableFromEntries()
    {
        // Clean highscore table from entries
        for (int i = 0; i < entryContainer.childCount; i++)
        {
            // Get a reference to the child game object
            GameObject child = entryContainer.GetChild(i).gameObject;

            // Destroy the child game object
            Destroy(child);
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        Debug.Log("CreateHighscoreEntryTransform");
        float templateHeight = 20f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);

        int rank = transformList.Count + 1;
        string rankString;
        var trophy = entryTransform.Find("Trophy").gameObject;
        var trophyImage = trophy.transform.Find("TrophyImage").GetComponent<Image>();
        var trophyBorder = trophy.transform.Find("Border").GetComponent<Image>();
        switch (rank)
        {
            case 1:
                rankString = "1ST";
                trophyImage.sprite = trophySprite;
                trophyImage.color = trophy1Color;
                trophyBorder.color = trophy1Color;
                break;
            case 2:
                rankString = "2ND";
                trophyImage.sprite = trophySprite;
                trophyImage.color = trophy2Color;
                trophyBorder.color = trophy2Color;
                break;
            case 3:
                rankString = "3RD";
                trophyImage.sprite = trophySprite;
                trophyImage.color = trophy3Color;
                trophyBorder.color = trophy3Color;
                break;
            default:
                rankString = rank + "TH";
                trophy.SetActive(false);
                break;
        }

        var postionValue = entryTransform.Find("PositionValue").GetComponent<TextMeshProUGUI>();
        postionValue.text = rankString;

        int score = highscoreEntry.score;
        var scoreValue = entryTransform.Find("ScoreValue").GetComponent<TextMeshProUGUI>();
        scoreValue.text = score.ToString();

        string name = highscoreEntry.playerName;
        var nameValue = entryTransform.Find("NameValue").GetComponent<TextMeshProUGUI>();
        nameValue.text = name;


        if (!highscoreEntryHighlighted && HighlightHighscoreEntry(name, score))
        {
            highscoreEntryHighlighted = true;
            postionValue.color = entryHighlightColor;
            scoreValue.color = entryHighlightColor;
            nameValue.color = entryHighlightColor;
        }

        transformList.Add(entryTransform);
    }

    private bool HighlightHighscoreEntry(string playerName, int score)
    {
        string currentPlayerName = PlayerPrefs.GetString("PlayerName");
        int currentScore = scoreController.DetermineTotalScore();

        return currentPlayerName.Equals(playerName) && currentScore.Equals(score);
    }

    public bool IsHighscoreInTop10(LevelEnum levelName, float score)
    {
        Debug.Log("CanHighscoreBeAdded");
        List<HighscoreEntry> loadedHighscoredForLevel = PlayerPrefsRepository.Instance.LoadTop10HighscoresForLevel(levelName);

        if (loadedHighscoredForLevel.Count < 10)
        {
            return true;
        }

        HighscoreEntry lastEntry = loadedHighscoredForLevel[loadedHighscoredForLevel.Count - 1];
        return score > lastEntry.score;
    }

    public void ShowHighscores(LevelEnum levelName)
    {
        List<HighscoreEntry> entries = PlayerPrefsRepository.Instance.LoadTop10HighscoresForLevel(levelName);
        levelNameText.text = HintController.Instance.currentLevel.GetName();

        CleanHighscoreTableFromEntries();
        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry entry in entries)
        {
            CreateHighscoreEntryTransform(entry, entryContainer, highscoreEntryTransformList);
        }
    }
}
