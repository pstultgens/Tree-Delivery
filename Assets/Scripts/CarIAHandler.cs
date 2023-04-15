using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CarIAHandler : MonoBehaviour
{
    public enum AIMode { followPlayer, followWaypoints };

    [Header("AI Settings")]
    [SerializeField] public AIMode aiMode;

    private Vector3 targetPosition = Vector3.zero;
    private Transform targetTransform = null;

    // Waypoints
    private WaypointNode currentWaypoint = null;
    private WaypointNode[] allWaypoints;

    private CarController carController;

    private void Awake()
    {
        carController = GetComponent<CarController>();
        allWaypoints = FindObjectsOfType<WaypointNode>();
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
            case AIMode.followWaypoints:
                FollowWaypoints();
                break;
        }

        inputVector.x = TurnTowardTarget();
        inputVector.y = 0.1f;

        // Send the input to the car controller.
        carController.SetInputVector(inputVector);
    }

    private void FollowPlayer()
    {
        if (targetTransform == null)
        {
            targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
        }
    }

    private void FollowWaypoints()
    {
        // Pick the closest waypoint if we don't have a waypoint set
        if (currentWaypoint == null)
        {
            currentWaypoint = FindClosestWaypoint();
        }

        if(currentWaypoint != null)
        {
            targetPosition = currentWaypoint.transform.position;

            // Store how close we are to the target
            float distanceToWaypoint = (targetPosition - transform.position).magnitude;

            // Check if we are close enough to consider that we have reached the waypoint
            if(distanceToWaypoint <= currentWaypoint.minimalDistanceToReachWaypoint)
            {
                // If we are close enough then follow to the next waypoint, if there are multiple waypoints then pick one at random
                currentWaypoint = currentWaypoint.nextWaypointNode[Random.Range(0, currentWaypoint.nextWaypointNode.Length)];
            }
        }

    }

    private WaypointNode FindClosestWaypoint()
    {
        return allWaypoints
                    .OrderBy(t => Vector3.Distance(transform.position, t.transform.position))
                    .FirstOrDefault();
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
