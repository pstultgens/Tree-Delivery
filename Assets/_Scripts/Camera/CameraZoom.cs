using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    
    [SerializeField] float zoomedOutSize = 30f; // The camera size when zoomed out
    [SerializeField] float zoomedInSize = 13f; // The camera size when zoomed in
    [SerializeField] float zoomSpeed = 1f; // The speed of the zoom
    [SerializeField] float moveSpeed = 5f; // The speed of the movement
    [SerializeField] float smoothTime = 0.2f; // The smoothing time for the zoom
    [SerializeField] Transform minimapZoomLocation;

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float targetSize;
    private float t = 0f;
    private bool isZooming = false;
    private float currentVelocity;
    private GameObject player;
    private Vector3 targetPosition;

    private void Start()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        player = FindAnyObjectByType<CarController>().gameObject;

        transform.position = minimapZoomLocation.position + new Vector3(0, 0, -10f);

        cinemachineVirtualCamera.Follow = null;
        cinemachineVirtualCamera.LookAt = null;
    }

    private void Update()
    {
        if (isZooming)
        {
            // Increment the interpolation parameter based on time
            t += Time.deltaTime * zoomSpeed;

            // Perform the camera size interpolation
            cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.SmoothDamp(cinemachineVirtualCamera.m_Lens.OrthographicSize, targetSize, ref currentVelocity, smoothTime);

            //transform.position = Vector3.MoveTowards(transform.position, targetPosition + new Vector3(0, 0, -10f), moveSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, targetPosition + new Vector3(0, 0, -10f), moveSpeed * Time.deltaTime);
            // If the interpolation parameter exceeds 1, stop the zoom
            if (t >= 1f)
            {
                StopZoom();
            }
        }
    }

    public void ZoomIn()
    {
        targetSize = zoomedInSize;
        targetPosition = player.transform.position;
        //cinemachineVirtualCamera.Follow = player.transform;
        //cinemachineVirtualCamera.LookAt = player.transform;
        
        StartZoom();
    }

    public void ZoomOut()
    {
        targetPosition = minimapZoomLocation.position;
        cinemachineVirtualCamera.Follow = null;
        cinemachineVirtualCamera.LookAt = null;
        targetSize = zoomedOutSize;
        StartZoom();
    }

    private void StartZoom()
    {
        isZooming = true;
        t = 0f;
    }

    private void StopZoom()
    {
        isZooming = false;
        cinemachineVirtualCamera.Follow = player.transform;
        cinemachineVirtualCamera.LookAt = player.transform;
    }
}
