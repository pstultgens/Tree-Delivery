using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class FeedbacksManager : MonoBehaviour
{
    public static FeedbacksManager Instance;

    [SerializeField] public MMFeedbacks booster;
    [SerializeField] public MMFeedbacks oilTrap;
    [SerializeField] public MMFeedbacks spikeTrap;
    [SerializeField] public MMFeedbacks spotDelivered;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
