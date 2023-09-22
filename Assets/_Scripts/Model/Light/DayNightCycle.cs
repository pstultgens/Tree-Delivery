using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] public bool enableDayToNight;
    [SerializeField] public float nightIntensity = 0.3f;
    [SerializeField] public float dayIntensity = 1f;
    [SerializeField] public float duration = 60f;

    private Light2D globalLight;
    private float elapsedTime = 0f;

    void Start()
    {
        globalLight = GetComponent<Light2D>();
    }

    void Update()
    {
        if (!enableDayToNight || SceneManager.isGamePaused || SceneManager.isCountingDown)
        {
            return;
        }

        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float newIntensity = Mathf.Lerp(dayIntensity, nightIntensity, t);

            globalLight.intensity = newIntensity;
        }
    }
}
