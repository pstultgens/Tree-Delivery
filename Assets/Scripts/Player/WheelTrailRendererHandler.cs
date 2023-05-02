using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrailRendererHandler : MonoBehaviour
{
    private CarController carController;
    TrailRenderer trailRenderer;

    private void Awake()
    {
        carController = GetComponentInParent<CarController>();
        trailRenderer = GetComponent<TrailRenderer>();

        trailRenderer.emitting = false;
    }    

    void Update()
    {
        if (carController.IsTireScreeching(out float lateralVelocity, out bool isBraking) || carController.inOilTrapMode || carController.inSpikeTrapMode)
        {
            trailRenderer.emitting = true;
        }
        else
        {
            trailRenderer.emitting = false;
        }
    }
}
