using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelParticleHandler : MonoBehaviour
{
    private float particleEmissionRate = 0;

    CarController carController;
    ParticleSystem particleSystemSmoke;
    ParticleSystem.EmissionModule particleSystemEmissionModule;

    private void Awake()
    {
        carController = GetComponentInParent<CarController>();
        particleSystemSmoke = GetComponent<ParticleSystem>();
        particleSystemEmissionModule = particleSystemSmoke.emission;


        particleSystemEmissionModule.rateOverTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Reduce the particles over time
        particleEmissionRate = Mathf.Lerp(particleEmissionRate, 0, Time.deltaTime * 5);
        particleSystemEmissionModule.rateOverTime = particleEmissionRate;

        if (carController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            // If the car tires are screeching then we will emitt smoke.
            // If the player is braking then emitt a lot of smoke.
            if (isBraking)
            {
                particleEmissionRate = 30;
            }
            else
            {
                // If the player is drifting we will emitt smoke based on how much the player is drifting.
                particleEmissionRate = Mathf.Abs(lateralVelocity) * 2;
            }
        }
    }
}
