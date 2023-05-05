using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UILevelCompleteScore : MonoBehaviour
{
    private TextMeshProUGUI scoreText;

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        scoreText.text = ScoreController.currentScore.ToString();
    }
}
