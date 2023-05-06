using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

public class CountDownController : MonoBehaviour
{
    [Header("Audio Mixers")]
    [SerializeField] public AudioMixer audioMixer;

    [Header("Audio sources")]
    [SerializeField] public AudioSource countDownAudioSource;
    [SerializeField] public AudioSource goAudioSource;

    private int countDownTime = 3;
    private TextMeshProUGUI countDownText;

    private void Awake()
    {
        countDownText = GetComponent<TextMeshProUGUI>();
        SceneManager.isCountingDown = true;
    }

    private void Start()
    {
        StartCoroutine(CountDownToStartCoroutine());
    }

    private IEnumerator CountDownToStartCoroutine()
    {

        while (countDownTime > 0)
        {
            yield return new WaitUntil(() => !SceneManager.isGamePaused);
            PlayCountDownSFX();
            countDownText.text = countDownTime.ToString();
            yield return new WaitForSeconds(1f);
            countDownTime--;
        }
        PlayGoSFX();
        countDownText.text = "GO";
        SceneManager.isCountingDown = false;

        yield return new WaitForSeconds(1f);
        countDownText.gameObject.SetActive(false);
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
