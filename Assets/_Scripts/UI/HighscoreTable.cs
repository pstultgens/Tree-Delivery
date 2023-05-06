using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class HighscoreTable : MonoBehaviour
{
    private static string HIGHSCORE_TABLE = "HighscoreTable_";

    [SerializeField] TextMeshProUGUI levelNameText;
    [SerializeField] Transform entryContainer;
    [SerializeField] Transform entryTemplate;

    private List<Transform> highscoreEntryTransformList;

    private void Awake()
    {
        // For Debug
        //if (!PlayerPrefs.HasKey(HIGHSCORE_TABLE))
        //{
        //    return;
        //}
        //List<HighscoreEntry> loadedHighscoredForLevel = LoadTop10HighscoresForLevel(levelName);

        //AddHighscoreEntry(LevelDifficultyEnum.Test, 1, "Philipp");
        //AddHighscoreEntry(LevelDifficultyEnum.Test, 2, "Philipp");
        //AddHighscoreEntry(LevelDifficultyEnum.Test, 3, "Philipp");
        //AddHighscoreEntry(LevelDifficultyEnum.Test, 4, "Philipp");
        //AddHighscoreEntry(LevelDifficultyEnum.Test, 5, "Philipp");
        //AddHighscoreEntry(LevelDifficultyEnum.Test, 6, "Philipp");
        //AddHighscoreEntry(LevelDifficultyEnum.Test, 7, "Philipp");
        //AddHighscoreEntry(LevelDifficultyEnum.Test, 8, "Philipp");
        //AddHighscoreEntry(LevelDifficultyEnum.Test, 9, "Philipp");
        //AddHighscoreEntry(LevelDifficultyEnum.Test, 10, "Philipp");
        //AddHighscoreEntry(LevelDifficultyEnum.Test, 11, "Philipp");

        //ShowHighscores(loadedHighscoredForLevel);
    }


    private List<HighscoreEntry> LoadTop10HighscoresForLevel(LevelEnum levelName)
    {
        Debug.Log("LoadHighscoresForLevel: " + levelName.GetName());
        if (!PlayerPrefs.HasKey(HIGHSCORE_TABLE + levelName))
        {
            return new List<HighscoreEntry>();
        }

        string jsonString = PlayerPrefs.GetString(HIGHSCORE_TABLE + levelName);
        Highscores loadedAllHighscores = JsonUtility.FromJson<Highscores>(jsonString);
        Debug.Log("loadedAllHighscores entries " + loadedAllHighscores.highscoreEntryList.Count);

        // Filter and order list for levelName
        List<HighscoreEntry> filterAndSortListForLevelName = loadedAllHighscores.highscoreEntryList            
            .OrderByDescending(e => e.score)
            .Take(10)
            .ToList();

        Debug.Log("Load Top10 for level: " + levelName.GetName() + ", size: " + filterAndSortListForLevelName.Count);
        return filterAndSortListForLevelName;
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
        switch (rank)
        {
            case 1:
                rankString = "1ST";
                break;
            case 2:
                rankString = "2ND";
                break;
            case 3:
                rankString = "3RD";
                break;
            default:
                rankString = rank + "TH";
                break;
        }

        entryTransform.Find("PositionValue").GetComponent<TextMeshProUGUI>().text = rankString;

        int randomScore = highscoreEntry.score;
        entryTransform.Find("ScoreValue").GetComponent<TextMeshProUGUI>().text = randomScore.ToString();

        string name = highscoreEntry.playerName;
        entryTransform.Find("NameValue").GetComponent<TextMeshProUGUI>().text = name;

        transformList.Add(entryTransform);
    }

    public bool IsHighscoreInTop10(LevelEnum levelName, int score)
    {
        Debug.Log("CanHighscoreBeAdded");
        List<HighscoreEntry> loadedHighscoredForLevel = LoadTop10HighscoresForLevel(levelName);

        if (loadedHighscoredForLevel.Count < 10)
        {
            return true;
        }

        HighscoreEntry lastEntry = loadedHighscoredForLevel[loadedHighscoredForLevel.Count - 1];
        return score > lastEntry.score;
    }

    public void AddHighscoreEntry(LevelEnum levelName, int score, string name)
    {
        Debug.Log("AddHighscoreEntry");
        // Create Highscore entry
        HighscoreEntry entry = new HighscoreEntry
        {
            score = score,
            playerName = name
        };

        // Load excisting Highscores
        List<HighscoreEntry> top10Highscores = LoadTop10HighscoresForLevel(levelName);

        // Add new entry to Highscores
        top10Highscores.Add(entry);
        List<HighscoreEntry> newList = top10Highscores.OrderByDescending(e => e.score).Take(10).ToList();

        // Save updated Highscores
        Highscores highscores = new Highscores();
        highscores.highscoreEntryList = newList;
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString(HIGHSCORE_TABLE + levelName, json);
        PlayerPrefs.Save();
    }

    public void ShowHighscores(LevelEnum levelName)
    {
        List<HighscoreEntry> entries = LoadTop10HighscoresForLevel(levelName);
        levelNameText.text = DifficultyController.Instance.currentLevelDifficulty.GetName();

        CleanHighscoreTableFromEntries();
        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry entry in entries)
        {
            CreateHighscoreEntryTransform(entry, entryContainer, highscoreEntryTransformList);
        }
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string playerName;
    }

}
