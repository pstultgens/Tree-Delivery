using UnityEngine;
using TMPro;

public class UIPackage : MonoBehaviour
{
    [SerializeField] GameObject imageCorrectDeliveredGameObject;
    [SerializeField] GameObject imageWrongDeliveredGameObject;

    private void Awake()
    {
        imageCorrectDeliveredGameObject.SetActive(false);
        imageWrongDeliveredGameObject.SetActive(false);
    }

    public void CorrectDelivered()
    {
        imageWrongDeliveredGameObject.SetActive(false);
        imageCorrectDeliveredGameObject.SetActive(true);
    }

    public void WrongDelivered()
    {
        imageCorrectDeliveredGameObject.SetActive(false);
        imageWrongDeliveredGameObject.SetActive(true);
    }

    public void PackagePickedup()
    {
        imageCorrectDeliveredGameObject.SetActive(false);
        imageWrongDeliveredGameObject.SetActive(false);
    }

    public int Value()
    {
        TextMeshProUGUI tmPro = GetComponentInChildren<TextMeshProUGUI>();
        return int.Parse(tmPro.text);
    }
}
