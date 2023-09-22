using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FireflyVFX : MonoBehaviour
{
    [SerializeField] float lightsOnIntensity = 0.8f;

    private ParticleSystem fireflyParticleSystem;

    private DayNightCycle dayNightCycle;
    private Light2D globalLight;
    private float currentIntensity;

    void Start()
    {
        dayNightCycle = FindObjectOfType<DayNightCycle>();
        fireflyParticleSystem = GetComponentInChildren<ParticleSystem>();

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
            fireflyParticleSystem.Play();
        }
        else
        {
            fireflyParticleSystem.Stop();
        }
    }

    private void DetermineCurrentIntensity()
    {
        currentIntensity = globalLight.intensity;
    }

}
