using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed = -30f;

    private void Update()
    {
        if (SceneManager.isGamePaused || SceneManager.isCountingDown)
        {
            return;
        }

        float rotationAngle = Time.deltaTime * rotationSpeed;
        transform.Rotate(0f, 0f, rotationAngle);
    }
}