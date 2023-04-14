using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] public Animator transition;
    [SerializeField] public float tranistionTime = 1f;

    
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevelCoroutine(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadLevel(int levelIndex)
    {
        StartCoroutine(LoadLevelCoroutine(levelIndex));
    }

    public void GoToScene(string sceneName)
    {
        StartCoroutine(LoadLevelCoroutine(sceneName));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadLevelCoroutine(int levelIndex)
    {
        // Play animation
        transition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(tranistionTime);

        // Load Scene
        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator LoadLevelCoroutine(string sceneName)
    {
        // Play animation
        transition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(tranistionTime);

        // Load Scene
        SceneManager.LoadScene(sceneName);
    }
}
