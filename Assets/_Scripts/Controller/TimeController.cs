using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimeController : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private float timer;
    private string formattedTime;

    private DeliveryController deliveryController;

    private void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        deliveryController = FindObjectOfType<DeliveryController>();
        timer = 0f;        
    }

    private void Update()
    {
        if (deliveryController.AllPackagesCorrectDelivered())
        {
            return;
        }

        if (SceneManager.isGamePaused || SceneManager.isCountingDown)
        {
            return;
        }

        IncreaseTimer();
    }

    public string GetFormattedTime()
    {
        return formattedTime;
    }

    public int GetElapsedTimeInSeconds()
    {
        TimeSpan timeSpan;
        if (TimeSpan.TryParseExact(formattedTime, "mm\\:ss\\:fff", null, out timeSpan))
        {
            return (int)timeSpan.TotalSeconds;
        }
        return 0;
    }

    private void IncreaseTimer()
    {
        timer += Time.deltaTime; // Increase the timer value by the elapsed time in seconds

        // Calculate minutes, seconds, and milliseconds
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        int milliseconds = Mathf.FloorToInt((timer * 1000f) % 1000f);

        // Format the timer display string
        formattedTime = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        timerText.text = formattedTime;
    }
}
