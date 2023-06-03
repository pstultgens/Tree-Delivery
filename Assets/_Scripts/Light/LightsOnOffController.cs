using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightsOnOffController : MonoBehaviour
{
    [SerializeField] public GameObject[] lights;
    [SerializeField] public float lightsOnIntensity = 0.8f;

    private DayNightCycle dayNightCycle;
    private Light2D globalLight;
    private float currentIntensity;

    void Start()
    {
        dayNightCycle = FindObjectOfType<DayNightCycle>();

        if (dayNightCycle == null)
        {
            Debug.Log("No GlobalLight2D in scene: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            return;
        }

        globalLight = dayNightCycle.GetComponent<Light2D>();
    }

    void Update()
    {
        if (globalLight == null)
        {
            return;
        }

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
    }

    private void TurnLightsOn()
    {
        foreach (GameObject light in lights)
        {
            light.SetActive(true);
        }
    }

    private void TurnLightsOff()
    {
        foreach (GameObject light in lights)
        {
            light.SetActive(false);
        }
    }
}
