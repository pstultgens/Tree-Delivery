using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIconHandler : MonoBehaviour
{
    private Camera minimapCamera;
    private float defaultMinimapSize = 100f;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;

        minimapCamera = GameObject.FindGameObjectWithTag("MinimapCamera").GetComponent<Camera>();

        transform.localScale = minimapCamera.orthographicSize / defaultMinimapSize * originalScale;
    }
}
