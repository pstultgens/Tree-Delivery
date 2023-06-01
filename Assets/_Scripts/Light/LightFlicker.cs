using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    public float minFlickerInterval = 0.1f; // Minimum interval between flickers
    public float maxFlickerInterval = 0.5f; // Maximum interval between flickers

    private Light2D light2D;
    private float originalIntensity;
    private bool isFlickering = false;
    private float flickerTimer;

    private void Start()
    {
        light2D = GetComponent<Light2D>();
        originalIntensity = light2D.intensity;
        flickerTimer = GetRandomFlickerInterval();
    }

    private void Update()
    {
        flickerTimer -= Time.deltaTime;

        if (flickerTimer <= 0f)
        {
            if (isFlickering)
            {
                StopFlickering();
            }
            else
            {
                StartFlickering();
            }

            flickerTimer = GetRandomFlickerInterval();
        }
    }

    private float GetRandomFlickerInterval()
    {
        return Random.Range(minFlickerInterval, maxFlickerInterval);
    }

    private void StartFlickering()
    {
        isFlickering = true;
        light2D.intensity = 0.4f; // Set the light intensity to a low value
    }

    private void StopFlickering()
    {
        isFlickering = false;
        light2D.intensity = originalIntensity; // Restore the original light intensity
    }
}
