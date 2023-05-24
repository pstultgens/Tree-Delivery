using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDataSettings : MonoBehaviour
{
    private static string PLAYER_PREFS_LEVEL = "Level_";
    private static string PLAYER_PREFS_HIGHSCORE_TABLE = "HighscoreTable_";

    private SceneManager sceneManager;

    private void Start()
    {
        sceneManager = FindObjectOfType<SceneManager>();

    }

    public void ResetData()
    {
        ResetAllLevels();
        ResetHighscores();       

        sceneManager.BackToMainMenu();
    }

    private void ResetAllLevels()
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

    private void ResetHighscores()
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


}
