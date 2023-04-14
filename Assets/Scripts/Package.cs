using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour
{
    [SerializeField] public GameObject minimapIcon;
    [SerializeField] public float pickUpDelay = 1f;
    [SerializeField] public bool isDelivered;

    private Collider2D collider2D;

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
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
        collider2D.enabled = false;
        yield return new WaitForSeconds(pickUpDelay);
        collider2D.enabled = true;
    }

}
