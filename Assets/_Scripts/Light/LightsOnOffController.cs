using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightsOnOffController : MonoBehaviour
{
    [SerializeField] public GameObject[] lights;
    [SerializeField] public float lightsOnIntensity = 0.8f;


    private Light2D globalLight;
    private float currentIntensity;

    void Start()
    {
        globalLight = FindObjectOfType<DayNightCycle>().GetComponent<Light2D>();
    }

    void Update()
    {
        DetermineCurrentIntensity();

        if (currentIntensity <= lightsOnIntensity)
        {
            TurnLightsOn();
        }
        else
        {
            TurnLightsOff();
        }
    }

    private void DetermineCurrentIntensity()
    {
        currentIntensity = globalLight.intensity;
        Debug.Log("Current itensity: " + currentIntensity);
    }

    private void TurnLightsOn()
    {
        Debug.Log("Turn Lights ON");
        foreach (GameObject light in lights)
        {
            light.SetActive(true);
        }
    }

    private void TurnLightsOff()
    {
        Debug.Log("Turn Lights OFF");
        foreach (GameObject light in lights)
        {
            light.SetActive(false);
        }
    }
}
