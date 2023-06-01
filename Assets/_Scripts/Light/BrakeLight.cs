using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BrakeLight : MonoBehaviour
{
    [SerializeField] public float brakeIntensity = 2.0f;
    [SerializeField] public float brakeOuterRadius = 3.0f;

    private CarController carController;
    private Light2D brakeLight;
    private float initialIntensity;
    private float initialOuterRadius;
    private Color32 initialBrakeColor;

    void Start()
    {
        carController = GetComponentInParent<CarController>();
        brakeLight = GetComponent<Light2D>();


        initialIntensity = brakeLight.intensity;
        initialOuterRadius = brakeLight.pointLightOuterRadius;
        initialBrakeColor = brakeLight.color;
    }

    void FixedUpdate()
    {
        carController.IsTireScreeching(out float lateralVelocity, out bool isBraking);
        bool isDrivingReverse = carController.IsDrivingReverse();

        if (isBraking && !isDrivingReverse)
        {
            // Is Braking
            brakeLight.color = initialBrakeColor;
            brakeLight.intensity = brakeIntensity;
            brakeLight.pointLightOuterRadius = brakeOuterRadius;
        }
        else if(!isBraking && isDrivingReverse)
        {
            // Is driving reverse
            brakeLight.color = Color.white;
            brakeLight.intensity = initialIntensity;
            brakeLight.pointLightOuterRadius = initialOuterRadius;
        }
        else
        {
            // Is not Braking or Driving reverse
            brakeLight.color = initialBrakeColor;
            brakeLight.intensity = initialIntensity;
            brakeLight.pointLightOuterRadius = initialOuterRadius;
        }
    }

}
