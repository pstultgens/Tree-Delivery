using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarIAHandler : MonoBehaviour
{
    public enum AIMode { followPlayer, followWaypoints };

    [Header("AI Settings")]
    [SerializeField] public AIMode aiMode;

    private Vector3 targetPosition = Vector3.zero;
    private Transform targetTransform = null;

    private CarController carController;

    private void Awake()
    {
        carController = GetComponent<CarController>();
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        Vector2 inputVector = Vector2.zero;

        switch (aiMode)
        {
            case AIMode.followPlayer:
                FollowPlayer();
                break;
        }

        inputVector.x = TurnTowardTarget();
        inputVector.y = 0.5f;

        // Send the input to the car controller.
        carController.SetInputVector(inputVector);
    }

    private void FollowPlayer()
    {
        if(targetTransform == null)
        {
            targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if(targetTransform != null)
        {
            targetPosition = targetTransform.position;
        }
    }

    private float TurnTowardTarget()
    {
        Vector2 vectorToTarget = targetPosition - transform.position;
        vectorToTarget.Normalize();

        // Calculate an angle towards the target
        float angleToTarget = Vector2.SignedAngle(transform.up, vectorToTarget);
        angleToTarget *= -1;

        // We want the car to turn as much as possible if the angle is greater than 45 degrees and we want it to smooth out so if the angle is small we want the AI to make smaller adjustments
        float steerAmount = angleToTarget / 45.0f;

        // Clamp steering to between -1 and 1
        steerAmount = Mathf.Clamp(steerAmount, -1.0f, 1.0f);

        return steerAmount;
    }

}
