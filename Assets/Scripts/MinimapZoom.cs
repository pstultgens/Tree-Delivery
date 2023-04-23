using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MinimapZoom : MonoBehaviour
{
    [SerializeField] private Transform zoomLocation;
    [SerializeField] private float zoomOutFactor = 2.0f;
    [SerializeField] private float zoomSpeed = 10f;

    private PlayerInputActions playerActions;
    private Camera minimapCamera;
    private FollowCamera followCamera;

    private float originalSize;
    private float zoomFactor = 1f;



    private void Start()
    {
        minimapCamera = GetComponent<Camera>();
        followCamera = GetComponent<FollowCamera>();
        originalSize = minimapCamera.orthographicSize;
    }

    private void OnEnable()
    {
        playerActions = new PlayerInputActions();
        playerActions.Enable();

        playerActions.Player.Minimap.performed += Zoom;
        playerActions.Player.Minimap.canceled += Zoom;
    }

    private void OnDisable()
    {
        playerActions.Enable();
    }

    private void Update()
    {
        float targetSize = originalSize * zoomFactor;
        if (targetSize != minimapCamera.orthographicSize)
        {
            minimapCamera.orthographicSize = Mathf.Lerp(minimapCamera.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
            minimapCamera.transform.position = Vector3.MoveTowards(minimapCamera.transform.position, zoomLocation.position + new Vector3(0, 0, -10f), Time.deltaTime * zoomSpeed * 35f);
        }
    }

    private void Zoom(InputAction.CallbackContext context)
    {
        if (followCamera == null)
        {
            return;
        }

        if (context.performed)
        {
            followCamera.enabled = false;
            zoomFactor = zoomOutFactor;
        }
        else if (context.canceled)
        {
            followCamera.enabled = true;
            zoomFactor = 1f;
        }
    }
}
