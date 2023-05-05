using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    private static int sortingOrder;

    [SerializeField] Vector3 moveSpeed = new Vector3(5f, 10f, 0f);
    [SerializeField] float moveBackSpeed = 10f;
    [SerializeField] float disappearTimer = 1f;
    [SerializeField] float disappearSpeed = 3f;
    [SerializeField] float increaseScaleAmount = 1f;
    [SerializeField] float decreaseScaleAmount = 1f;

    [SerializeField] Color32 postiveNumberColor = new Color32(1, 1, 1, 1);
    [SerializeField] Color32 negativeNumberColor = new Color32(1, 1, 1, 1);

    private TextMeshPro textMeshPro;
    private Color textColor;
    private float disappearTimerMax;

    public void Awake()
    {
        textMeshPro = GetComponent<TextMeshPro>();
    }

    public void Setup(int scoreAmount)
    {
        if (scoreAmount > 0)
        {
            textColor = postiveNumberColor;
            textMeshPro.SetText("+" + scoreAmount.ToString());
        }
        else
        {
            textColor = negativeNumberColor;
            textMeshPro.SetText(scoreAmount.ToString());
        }
        textMeshPro.color = textColor;
        disappearTimerMax = disappearTimer;

        sortingOrder++;
        textMeshPro.sortingOrder = sortingOrder;
    }

    void Update()
    {
        transform.position += moveSpeed * Time.deltaTime;
        moveSpeed -= moveSpeed * moveBackSpeed * Time.deltaTime;

        if (disappearTimer > disappearTimerMax / 2)
        {
            // First half of the popup lifetime
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            // Second half of the popup lifetime
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer <= 0)
        {
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMeshPro.color = textColor;

            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
