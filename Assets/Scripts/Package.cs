using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour
{
    [SerializeField] public GameObject minimapIcon;
    [SerializeField] public float pickUpDelay = 1f;
    [SerializeField] public bool isDelivered;

    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
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
        this.gameObject.SetActive(false);
    }

    IEnumerator PickUpDelayCoroutine()
    {
        boxCollider.enabled = false;
        yield return new WaitForSeconds(pickUpDelay);
        boxCollider.enabled = true;
    }

}
