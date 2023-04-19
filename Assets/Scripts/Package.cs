using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Package : MonoBehaviour
{
    [SerializeField] public GameObject minimapIcon;
    [SerializeField] public float pickUpDelay = 1f;
    [SerializeField] public bool isDelivered;

    private BoxCollider2D boxCollider;
    private UIController uiController;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
    }

    public int Value()
    {
        TextMeshPro tmPro = GetComponentInChildren<TextMeshPro>();
        return int.Parse(tmPro.text);
    }

    public void Pickedup()
    {
        this.gameObject.SetActive(false);
    }

    public void Drop(Vector2 dropLocation)
    {
        transform.position = dropLocation;
        minimapIcon.SetActive(true);
        this.gameObject.SetActive(true);
        StartCoroutine(PickUpDelayCoroutine());
    }

    public void Delivered()
    {
        isDelivered = true;

        if (DifficultyController.showUIPackages)
        {
            uiController.PackageDelivered(Value());
        }

        this.gameObject.SetActive(false);
    }

    IEnumerator PickUpDelayCoroutine()
    {
        boxCollider.enabled = false;
        yield return new WaitForSeconds(pickUpDelay);
        boxCollider.enabled = true;
    }

}
