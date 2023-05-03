using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public static int currentScore;

    [SerializeField] public int startScore = 99999;
    [SerializeField] public int scoreDelayAmount = 1;

    private TextMeshProUGUI scoringText;
    private float timer;

    private void Awake()
    {
        scoringText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= scoreDelayAmount)
        {
            timer = 0f;
            startScore -= 1;
        }

        scoringText.text = startScore.ToString();
        currentScore = startScore;
    }
}
