using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAlphaFade : MonoBehaviour
{
    [SerializeField] float fadeDuration = 2f; // The duration of the fade in seconds

    private float fadeTimer = 0f;
    private bool isFading = false;
    private Image image;
    private Color originalColor;

    private void Start()
    {
        image = GetComponent<Image>();
        originalColor = image.color;

        StartFade();
    }

    private void Update()
    {
        if (isFading)
        {
            fadeTimer += Time.deltaTime;

            // Calculate the current alpha value based on the elapsed time and fade duration
            float currentAlpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);

            // Set the alpha value of the material color
            Color currentColor = new Color(originalColor.r, originalColor.g, originalColor.b, currentAlpha);

            // Apply the current color to the material
            image.color = currentColor;

            if (fadeTimer >= fadeDuration)
            {
                StopFade();
            }
        }
    }

    public void StartFade()
    {
        fadeTimer = 0f;
        isFading = true;
    }

    public void StopFade()
    {
        isFading = false;

        // Ensure the final color is fully transparent (alpha = 0)
        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}
