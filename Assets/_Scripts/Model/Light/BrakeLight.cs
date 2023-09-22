using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BrakeLight : MonoBehaviour
{
    [Header("Spot")]
    [SerializeField] float spotBrakeIntensity = 2.0f;
    [SerializeField] float spotBrakeOuterRadius = 3.0f;

    [Header("Bulb")]
    [SerializeField] Light2D bulbBrakeLight;
    [SerializeField] float bulbBrakeIntensity = 3.0f;

    private CarController carController;

    private Light2D spotBrakeLight;
    private float spotInitialIntensity;
    private float spotInitialOuterRadius;
    private Color32 spotInitialBrakeColor;

    private float bulbInitialIntensity;
    private Color32 bulbInitialBrakeColor;

    void Start()
    {
        carController = GetComponentInParent<CarController>();

        spotBrakeLight = GetComponent<Light2D>();
        spotInitialIntensity = spotBrakeLight.intensity;
        spotInitialOuterRadius = spotBrakeLight.pointLightOuterRadius;
        spotInitialBrakeColor = spotBrakeLight.color;

        bulbInitialIntensity = bulbBrakeLight.intensity;
        bulbInitialBrakeColor = bulbBrakeLight.color;
    }

    void FixedUpdate()
    {
        carController.IsTireScreeching(out float lateralVelocity, out bool isBraking);
        bool isDrivingReverse = carController.IsDrivingReverse();

        if (isBraking && !isDrivingReverse)
        {
            // Is Braking
            spotBrakeLight.color = spotInitialBrakeColor;
            spotBrakeLight.intensity = spotBrakeIntensity;
            spotBrakeLight.pointLightOuterRadius = spotBrakeOuterRadius;

            bulbBrakeLight.color = bulbInitialBrakeColor;
            bulbBrakeLight.intensity = bulbBrakeIntensity;
        }
        else if (!isBraking && isDrivingReverse)
        {
            // Is driving reverse
            spotBrakeLight.color = Color.white;
            spotBrakeLight.intensity = spotInitialIntensity;
            spotBrakeLight.pointLightOuterRadius = spotInitialOuterRadius;

            bulbBrakeLight.color = Color.white;
            bulbBrakeLight.intensity = bulbInitialIntensity;
        }
        else
        {
            // Is not Braking or Driving reverse
            spotBrakeLight.color = spotInitialBrakeColor;
            spotBrakeLight.intensity = spotInitialIntensity;
            spotBrakeLight.pointLightOuterRadius = spotInitialOuterRadius;

            bulbBrakeLight.color = bulbInitialBrakeColor;
            bulbBrakeLight.intensity = bulbInitialIntensity;
        }
    }

}
