using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public class PlayerPrefsRepository : MonoBehaviour
{
    public static PlayerPrefsRepository Instance { get; private set; }

    private static string PLAYER_PREFS_PLAYER_NAME = "PlayerName";
    private static string PLAYER_PREFS_LEVEL = "Level_";
    private static string PLAYER_PREFS_HIGHSCORE_TABLE = "HighscoreTable_";

    private static string MUSIC_VOLUME = "MusicVolume";
    private static string SFX_VOLUME = "SFXVolume";
    private static string CAMERA_SHAKE = "CameraShake";
    private static string FULLSCREEN = "Fullscreen";

    private PlayerInputActions playerActions;
    private bool cheat1Pressed;
    private bool cheat2Pressed;
    private bool cheat3Pressed;

    private void OnEnable()
    {
        playerActions.Enable();
        playerActions.Player.Cheat1.started += OnCheat1Pressed;
        playerActions.Player.Cheat2.started += OnCheat2Pressed;
        playerActions.Player.Cheat3.started += OnCheat3Pressed;
    }

    private void OnDisable()
    {
        playerActions.Disable();
        playerActions.Player.Cheat1.canceled -= OnCheat1Pressed;
        playerActions.Player.Cheat2.canceled -= OnCheat2Pressed;
        playerActions.Player.Cheat3.canceled -= OnCheat3Pressed;
    }

    private void Awake()
    {
        playerActions = new PlayerInputActions();

        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }       
    } 

    private void Update()
    {
        if (cheat1Pressed && cheat2Pressed && cheat3Pressed)
        {
            UnlockAndCompleteAllLevels();
        }
    }

    private void OnCheat1Pressed(InputAction.CallbackContext context)
    {
        cheat1Pressed = true;
    }
    private void OnCheat2Pressed(InputAction.CallbackContext context)
    {
        cheat2Pressed = true;
    }
    private void OnCheat3Pressed(InputAction.CallbackContext context)
    {
        cheat3Pressed = true;
    }

    public HighscoreEntry GetHighscore(LevelEnum level)
    {
        if (!PlayerPrefs.HasKey(PLAYER_PREFS_HIGHSCORE_TABLE + level))
        {
            
            return null;
        }

        string jsonString = PlayerPrefs.GetString(PLAYER_PREFS_HIGHSCORE_TABLE + level);
        Highscores loadedAllHighscores = JsonUtility.FromJson<Highscores>(jsonString);

        // Filter and order list for levelName
        return loadedAllHighscores.highscoreEntryList
            .OrderByDescending(e => e.score)
            .Take(1)
            .ToList()
            .First();
    } 

    public List<HighscoreEntry> LoadTop10HighscoresForLevel(LevelEnum levelName)
    {
        if (!PlayerPrefs.HasKey(PLAYER_PREFS_HIGHSCORE_TABLE + levelName))
        {
            return new List<HighscoreEntry>();
        }

        string jsonString = PlayerPrefs.GetString(PLAYER_PREFS_HIGHSCORE_TABLE + levelName);
        Highscores loadedAllHighscores = JsonUtility.FromJson<Highscores>(jsonString);

        // Filter and order list for levelName
        List<HighscoreEntry> filterAndSortListForLevelName = loadedAllHighscores.highscoreEntryList
            .OrderByDescending(e => e.score)
            .Take(10)
            .ToList();

        return filterAndSortListForLevelName;
    }

    public void AddHighscoreEntry(LevelEnum levelName, int score)
    {
        // Create Highscore entry
        HighscoreEntry entry = new HighscoreEntry
        {
            score = score,
            playerName = GetPlayerName()
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
        PlayerPrefs.SetString(PLAYER_PREFS_HIGHSCORE_TABLE + levelName, json);
        PlayerPrefs.Save();
    }

    public void AddPlayerName(string name)
    {
        PlayerPrefs.SetString(PLAYER_PREFS_PLAYER_NAME, name);
        PlayerPrefs.Save();
    }

    public string GetPlayerName()
    {
        if (PlayerPrefs.HasKey(PLAYER_PREFS_PLAYER_NAME))
        {
            return PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME);

        }
        return "Debugger";
    }

    public bool IsLevelUnlocked(LevelEnum level)
    {
        if (LevelEnum.Tutorial.Equals(level))
        {
            // Tutorial Level is always unlocked
            return true;
        }

        if (PlayerPrefs.HasKey(PLAYER_PREFS_LEVEL + level))
        {
            string jsonString = PlayerPrefs.GetString(PLAYER_PREFS_LEVEL + level);
            Level loadedLevel = JsonUtility.FromJson<Level>(jsonString);

            return loadedLevel.unlocked;
        }
        return false;
    }

    public bool IsLevelFinished(LevelEnum level)
    {
        if (PlayerPrefs.HasKey(PLAYER_PREFS_LEVEL + level))
        {
            string jsonString = PlayerPrefs.GetString(PLAYER_PREFS_LEVEL + level);
            Level loadedLevel = JsonUtility.FromJson<Level>(jsonString);

            return loadedLevel.finished;
        }
        return false;
    }

    public void UpdateFinishedLevel()
    {
        if (PlayerPrefs.HasKey(PLAYER_PREFS_LEVEL + HintController.Instance.currentLevel))
        {
            string loadedJson = PlayerPrefs.GetString(PLAYER_PREFS_LEVEL + HintController.Instance.currentLevel);
            Level loadedLevel = JsonUtility.FromJson<Level>(loadedJson);
            loadedLevel.finished = true;
            string updatedJson = JsonUtility.ToJson(loadedLevel);
            PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + HintController.Instance.currentLevel, updatedJson);
            PlayerPrefs.Save();
        }
        else
        {
            Level level = new Level();
            level.unlocked = true;
            level.finished = true;
            string json = JsonUtility.ToJson(level);
            PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + HintController.Instance.currentLevel, json);
            PlayerPrefs.Save();
        }
    }

    public void UnlockNextLevel()
    {
        LevelEnum nextLevel = HintController.Instance.DetermineNextLevel();

        if (!PlayerPrefs.HasKey(PLAYER_PREFS_LEVEL + nextLevel))
        {
            Level level = new Level();
            level.unlocked = true;
            string json = JsonUtility.ToJson(level);

            PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + nextLevel, json);
            PlayerPrefs.Save();
        }
    }

    public bool AllEasyLevelsUnlocked()
    {
        if (PlayerPrefs.HasKey(PLAYER_PREFS_LEVEL + LevelEnum.Easy1)
            && PlayerPrefs.HasKey(PLAYER_PREFS_LEVEL + LevelEnum.Easy2)
            && PlayerPrefs.HasKey(PLAYER_PREFS_LEVEL + LevelEnum.Easy3)
            && PlayerPrefs.HasKey(PLAYER_PREFS_LEVEL + LevelEnum.Easy4)
            && PlayerPrefs.HasKey(PLAYER_PREFS_LEVEL + LevelEnum.Easy5)
            && PlayerPrefs.HasKey(PLAYER_PREFS_LEVEL + LevelEnum.Easy6)
            && PlayerPrefs.HasKey(PLAYER_PREFS_LEVEL + LevelEnum.Easy7))
        {
            return true;
        }
        return false;
    }

    public bool AllLevelsFinished()
    {
        return
            IsLevelFinished(LevelEnum.Tutorial) &&
            IsLevelFinished(LevelEnum.Test) &&
            IsLevelFinished(LevelEnum.Easy1) &&
            IsLevelFinished(LevelEnum.Easy2) &&
            IsLevelFinished(LevelEnum.Easy3) &&
            IsLevelFinished(LevelEnum.Easy4) &&
            IsLevelFinished(LevelEnum.Easy5) &&
            IsLevelFinished(LevelEnum.Easy6) &&
            IsLevelFinished(LevelEnum.Easy7) &&
            IsLevelFinished(LevelEnum.Hard1) &&
            IsLevelFinished(LevelEnum.Hard2) &&
            IsLevelFinished(LevelEnum.Hard3) &&
            IsLevelFinished(LevelEnum.Hard4) &&
            IsLevelFinished(LevelEnum.Hard5) &&
            IsLevelFinished(LevelEnum.Hard6) &&
            IsLevelFinished(LevelEnum.Hard7);
    }

    public void UnlockAllEasyLevels()
    {
        Level level = new Level();
        level.unlocked = true;
        string json = JsonUtility.ToJson(level);

        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Easy1, json);
        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Easy2, json);
        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Easy3, json);
        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Easy4, json);
        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Easy5, json);
        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Easy6, json);
        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Easy7, json);
        PlayerPrefs.Save();
    }

    private void UnlockAndCompleteAllLevels()
    {
        Level level = new Level();
        level.unlocked = true;
        level.finished = true;
        string json = JsonUtility.ToJson(level);

        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Tutorial, json);
        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Test, json);

        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Easy1, json);
        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Easy2, json);
        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Easy3, json);
        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Easy4, json);
        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Easy5, json);
        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Easy6, json);
        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Easy7, json);

        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Hard1, json);
        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Hard2, json);
        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Hard3, json);
        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Hard4, json);
        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Hard5, json);
        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Hard6, json);
        PlayerPrefs.SetString(PLAYER_PREFS_LEVEL + LevelEnum.Hard7, json);

        PlayerPrefs.Save();
    }

    public void SetFullscreenSetting(bool isOn)
    {
        PlayerPrefs.SetString(FULLSCREEN, isOn.ToString());
        PlayerPrefs.Save();
    }

    public bool LoadFullscreenSetting()
    {
        if (PlayerPrefs.HasKey(FULLSCREEN))
        {
            return bool.Parse(PlayerPrefs.GetString(FULLSCREEN));
        }
        return true;
    }

    public void SetCameraShakeSetting(bool isOn)
    {
        PlayerPrefs.SetString(CAMERA_SHAKE, isOn.ToString());
        PlayerPrefs.Save();
    }

    public bool LoadCameraShakeSetting()
    {
        if (PlayerPrefs.HasKey(CAMERA_SHAKE))
        {
            return bool.Parse(PlayerPrefs.GetString(CAMERA_SHAKE));
        }
        return true;
    }

    public void ResetAllLevels()
    {
        PlayerPrefs.DeleteKey(PLAYER_PREFS_LEVEL + LevelEnum.Tutorial);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_LEVEL + LevelEnum.Test);

        PlayerPrefs.DeleteKey(PLAYER_PREFS_LEVEL + LevelEnum.Easy1);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_LEVEL + LevelEnum.Easy2);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_LEVEL + LevelEnum.Easy3);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_LEVEL + LevelEnum.Easy4);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_LEVEL + LevelEnum.Easy5);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_LEVEL + LevelEnum.Easy6);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_LEVEL + LevelEnum.Easy7);

        PlayerPrefs.DeleteKey(PLAYER_PREFS_LEVEL + LevelEnum.Hard1);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_LEVEL + LevelEnum.Hard2);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_LEVEL + LevelEnum.Hard3);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_LEVEL + LevelEnum.Hard4);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_LEVEL + LevelEnum.Hard5);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_LEVEL + LevelEnum.Hard6);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_LEVEL + LevelEnum.Hard7);

        PlayerPrefs.Save();
    }

    public void ResetHighscores()
    {
        PlayerPrefs.DeleteKey(PLAYER_PREFS_HIGHSCORE_TABLE + LevelEnum.Easy1);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_HIGHSCORE_TABLE + LevelEnum.Easy2);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_HIGHSCORE_TABLE + LevelEnum.Easy3);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_HIGHSCORE_TABLE + LevelEnum.Easy4);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_HIGHSCORE_TABLE + LevelEnum.Easy5);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_HIGHSCORE_TABLE + LevelEnum.Easy6);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_HIGHSCORE_TABLE + LevelEnum.Easy7);

        PlayerPrefs.DeleteKey(PLAYER_PREFS_HIGHSCORE_TABLE + LevelEnum.Hard1);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_HIGHSCORE_TABLE + LevelEnum.Hard2);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_HIGHSCORE_TABLE + LevelEnum.Hard3);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_HIGHSCORE_TABLE + LevelEnum.Hard4);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_HIGHSCORE_TABLE + LevelEnum.Hard5);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_HIGHSCORE_TABLE + LevelEnum.Hard6);
        PlayerPrefs.DeleteKey(PLAYER_PREFS_HIGHSCORE_TABLE + LevelEnum.Hard7);

        PlayerPrefs.Save();
    }

    public float LoadMusicVolume()
    {
        if (PlayerPrefs.HasKey(MUSIC_VOLUME))
        {
            return PlayerPrefs.GetFloat(MUSIC_VOLUME);
        }
        return 0.25f;
    }

    public void SetMusicVolume(float volume)
    {     
        PlayerPrefs.SetFloat(MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float LoadSFXVolume()
    {
        if (PlayerPrefs.HasKey(SFX_VOLUME))
        {
            return PlayerPrefs.GetFloat(SFX_VOLUME);
        }
        return 0.25f;
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat(SFX_VOLUME, volume);
        PlayerPrefs.Save();
    }

}