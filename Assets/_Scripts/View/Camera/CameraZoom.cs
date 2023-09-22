using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{

    public float zoomOutSpeed = 0.25f;
    public float zoomInSpeed = 1.0f;
    public float maxZoom = 20.0f;
    public float velocityThreshold = 15.0f;

    private CinemachineVirtualCamera virtualCamera;
    private Vector3 previousCameraPosition;
    private Vector3 currentVelocity;
    private float initialZoom;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        previousCameraPosition = virtualCamera.transform.position;
        initialZoom = virtualCamera.m_Lens.OrthographicSize;
    }

    private void Update()
    {
        // Calculate the current camera velocity
        Vector3 currentCameraPosition = virtualCamera.transform.position;
        Vector3 velocity = (currentCameraPosition - previousCameraPosition) / Time.deltaTime;

        // Smooth the velocity using exponential smoothing
        currentVelocity = Vector3.Lerp(currentVelocity, velocity, 0.1f);

        // Update the previous camera position for the next frame
        previousCameraPosition = currentCameraPosition;

        // Calculate the magnitude of the velocity
        float speed = currentVelocity.magnitude;

        // Check if the speed exceeds the threshold
        if (speed > velocityThreshold)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, maxZoom, zoomOutSpeed * Time.deltaTime);
        }
        else
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, initialZoom, zoomInSpeed * Time.deltaTime);
        }
    }
}
