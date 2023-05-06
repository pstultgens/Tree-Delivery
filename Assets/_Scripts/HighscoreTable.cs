using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class HighscoreTable : MonoBehaviour
{
    private static string HIGHSCORE_TABLE = "HighscoreTable";

    [SerializeField] string levelName = "Test";
    [SerializeField] Transform entryContainer;
    [SerializeField] Transform entryTemplate;

    private List<Transform> highscoreEntryTransformList;

    private void Awake()
    {
        //entryTemplate.gameObject.SetActive(false);

        if (!PlayerPrefs.HasKey(HIGHSCORE_TABLE))
        {
            return;
        }

        List<HighscoreEntry> loadedHighscoredForLevel = LoadTop10HighscoresForLevel(levelName);
        ShowHighscores(loadedHighscoredForLevel);

        //AddHighscoreEntry("Test", 123213, "Esmee");
        //AddHighscoreEntry("Test", 4654799, "Olivia");
        //AddHighscoreEntry("Test", 934324234, "MASTER");
        //AddHighscoreEntry("Test", 21367876, "Philipp");
        //AddHighscoreEntry("Test", 1, "A");
        //AddHighscoreEntry("Test", 2, "A");
        //AddHighscoreEntry("Test", 3, "A");
        //AddHighscoreEntry("Test", 4, "A");
        //AddHighscoreEntry("Test", 5, "A");
        //AddHighscoreEntry("Test", 994324234, "Winner");
        //AddHighscoreEntry("Test", 999994235, "Topper");
    }


    private List<HighscoreEntry> LoadTop10HighscoresForLevel(string levelName)
    {
        Debug.Log("LoadHighscoresForLevel: " + levelName);
        if (!PlayerPrefs.HasKey(HIGHSCORE_TABLE))
        {
            return new List<HighscoreEntry>();
        }

        string jsonString = PlayerPrefs.GetString(HIGHSCORE_TABLE);
        Highscores loadedAllHighscores = JsonUtility.FromJson<Highscores>(jsonString);
        Debug.Log("loadedAllHighscores entries " + loadedAllHighscores.highscoreEntryList.Count);

        // Filter and order list for levelName
        List<HighscoreEntry> filterAndSortListForLevelName = loadedAllHighscores.highscoreEntryList
            .Where(e => e.levelName.Equals(levelName))
            .OrderByDescending(e => e.score)
            .Take(10)
            .ToList();

        Debug.Log("Load Top10 for level: " + levelName + ", size: " + filterAndSortListForLevelName.Count);
        return filterAndSortListForLevelName;
    }

    private void ShowHighscores(List<HighscoreEntry> entries)
    {
        CleanHighscoreTableFromEntries();
        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry entry in entries)
        {
            CreateHighscoreEntryTransform(entry, entryContainer, highscoreEntryTransformList);
        }
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
        //entryTransform.gameObject.SetActive(true);

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

    public bool IsHighscoreInTop10(string levelName, int score)
    {
        Debug.Log("CanHighscoreBeAdded");
        List<HighscoreEntry> loadedHighscoredForLevel = LoadTop10HighscoresForLevel(levelName);
        HighscoreEntry lastEntry = loadedHighscoredForLevel[loadedHighscoredForLevel.Count - 1];

        return loadedHighscoredForLevel.Count < 10 || score > lastEntry.score;
    }

    public void AddHighscoreEntry(string levelName, int score, string name)
    {
        Debug.Log("AddHighscoreEntry");
        // Create Highscore entry
        HighscoreEntry entry = new HighscoreEntry
        {
            levelName = levelName,
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
        PlayerPrefs.SetString(HIGHSCORE_TABLE, json);
        PlayerPrefs.Save();

        ShowHighscores(newList);
    }

    [System.Serializable]
    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public string levelName;
        public int score;
        public string playerName;
    }

}
