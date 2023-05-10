using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

public class CountDownController : MonoBehaviour
{
    [Header("Audio Mixers")]
    [SerializeField] AudioMixer audioMixer;

    [Header("Audio sources")]
    [SerializeField] AudioSource countDownAudioSource;
    [SerializeField] AudioSource goAudioSource;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] TextMeshProUGUI levelNameText;

    private int countDownTime = 3;       

    private void Start()
    {        
        levelNameText.text = HintController.Instance.currentLevel.GetName();
        SceneManager.isCountingDown = true;
        StartCoroutine(CountDownToStartCoroutine());
    }

    private IEnumerator CountDownToStartCoroutine()
    {

        while (countDownTime > 0)
        {
            yield return new WaitUntil(() => !SceneManager.isGamePaused);
            PlayCountDownSFX();
            countText.text = countDownTime.ToString();
            yield return new WaitForSeconds(1f);
            countDownTime--;
        }
        PlayGoSFX();
        countText.text = "GO";
        SceneManager.isCountingDown = false;

        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private void PlayCountDownSFX()
    {
        countDownAudioSource.Play();
    }

    private void PlayGoSFX()
    {
        goAudioSource.Play();
    }
}
