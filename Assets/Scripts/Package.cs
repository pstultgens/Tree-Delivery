using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Package : MonoBehaviour
{
    [SerializeField] public GameObject minimapIcon;
    //[SerializeField] public float pickUpDelay = 1f;
    [SerializeField] public bool isCorrectDelivered;

    //private BoxCollider2D boxCollider;
    private UIController uiController;
    private MusicController musicController;
    private DifficultyController difficultyController;
    private GameObject player;

    private void Awake()
    {
        //boxCollider = GetComponent<BoxCollider2D>();
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        musicController = FindAnyObjectByType<MusicController>();
        difficultyController = FindAnyObjectByType<DifficultyController>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public int Value()
    {
        TextMeshPro tmPro = GetComponentInChildren<TextMeshPro>();
        return int.Parse(tmPro.text);
    }

    public void Pickedup()
    {
        musicController.PlayPickupSFX();

        uiController.PackagePickedup(Value());
        gameObject.SetActive(false);
    }

    public void Drop(Vector2 dropLocation)
    {
        musicController.PlayDropSFX();

        transform.position = dropLocation;
        minimapIcon.SetActive(true);
        gameObject.SetActive(true);
        //StartCoroutine(PickUpDelayCoroutine());
    }

    public void CorrectDelivered()
    {
        isCorrectDelivered = true;

        if (difficultyController.showHintUIPackageAndMinimap)
        {
            uiController.PackageCorrectDelivered(Value());
        }
        // Set delivered package at players location
        transform.position = player.transform.position;

        this.gameObject.SetActive(false);
    }

    public void WrongDelivered()
    {
        isCorrectDelivered = false;

        if (difficultyController.showHintUIPackageAndMinimap)
        {
            uiController.PackageWrongDelivered(Value());
        }
        // Set delivered package at players location
        transform.position = player.transform.position;

        this.gameObject.SetActive(false);
    }

    //IEnumerator PickUpDelayCoroutine()
    //{
    //    boxCollider.enabled = false;
    //    yield return new WaitForSeconds(pickUpDelay);
    //    boxCollider.enabled = true;
    //}

}
