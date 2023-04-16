using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CarIAHandler : MonoBehaviour
{
    public enum AIMode { followPlayer, followWaypoints, followMouse };

    [Header("AI Settings")]
    [SerializeField] public AIMode aiMode;
    [SerializeField] public float maxSpeed = 16f;
    [SerializeField] public bool isAvoidingCars = true;

    private Vector3 targetPosition = Vector3.zero;
    private Transform targetTransform = null;

    // Waypoints
    private WaypointNode currentWaypoint = null;
    private WaypointNode[] allWaypoints;

    // Avoidance
    private Vector2 avoidanceVectorLerp = Vector3.zero;

    // Components
    private CarController carController;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        carController = GetComponent<CarController>();
        allWaypoints = FindObjectsOfType<WaypointNode>();
        boxCollider = GetComponent<BoxCollider2D>();
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
            case AIMode.followMouse:
                FollowMouse();
                break;
        }

        inputVector.x = TurnTowardTarget();
        inputVector.y = ApplyThrottleOrBrake(inputVector.x);

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

        if (currentWaypoint != null)
        {
            targetPosition = currentWaypoint.transform.position;

            // Store how close we are to the target
            float distanceToWaypoint = (targetPosition - transform.position).magnitude;

            // Check if we are close enough to consider that we have reached the waypoint
            if (distanceToWaypoint <= currentWaypoint.minimalDistanceToReachWaypoint)
            {
                if (currentWaypoint.maxSpeed > 0)
                {
                    maxSpeed = currentWaypoint.maxSpeed;
                }
                else
                {
                    // Go as fast as possible
                    maxSpeed = 1000;
                }

                // If we are close enough then follow to the next waypoint, if there are multiple waypoints then pick one at random
                currentWaypoint = currentWaypoint.nextWaypointNode[Random.Range(0, currentWaypoint.nextWaypointNode.Length)];
            }
        }
    }

    // AI follows the mouse postion
    private void FollowMouse()
    {
        // Take the mouse position in screen space and convert it to the world space
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Set the target position for the AI
        targetPosition = worldPosition;
    }

    // Find the closest waypoint to the AI
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

        if (isAvoidingCars)
        {
            AvoidCars(vectorToTarget, out vectorToTarget);
        }

        // Calculate an angle towards the target
        float angleToTarget = Vector2.SignedAngle(transform.up, vectorToTarget);
        angleToTarget *= -1;

        // We want the car to turn as much as possible if the angle is greater than 45 degrees and we want it to smooth out so if the angle is small we want the AI to make smaller adjustments
        float steerAmount = angleToTarget / 45.0f;

        // Clamp steering to between -1 and 1
        steerAmount = Mathf.Clamp(steerAmount, -1.0f, 1.0f);

        return steerAmount;
    }

    private float ApplyThrottleOrBrake(float inputX)
    {
        // If we are going too fast then do not accelerate further
        if (carController.GetVelocityMagnitude() > maxSpeed)
        {
            return 0;
        }

        // Apply throttle forward based on how much the car wants to turn
        // If it's a sharp turn this will cause the to apply less speed forward
        return 1.05f - Mathf.Abs(inputX) / 1.0f;
    }

    // Checks for cars ahead of the car
    private bool IsCarInFrontOfAICar(out Vector3 otherCarPosition, out Vector3 otherCarRightVector)
    {
        // Disable the cars own collider to avoid having the AI car detect itself
        boxCollider.enabled = false;


        // Perform the circle cast in front of the car with a slight offset forward and only in the Car layer
        RaycastHit2D raycastHit = Physics2D.CircleCast(transform.position + transform.up * 0.5f, 1.0f, transform.up, 12, 1 << LayerMask.NameToLayer("Car"));

        // Enable the collider again so the car can collide and other cars can detect it
        boxCollider.enabled = true;

        if (raycastHit.collider != null)
        {
            // Draw a red line showing how long the detection is, make it red since we have detected another car
            Debug.DrawRay(transform.position, transform.up * 12, Color.red);

            otherCarPosition = raycastHit.collider.transform.position;
            otherCarRightVector = raycastHit.collider.transform.right;

            return true;
        }
        else
        {
            // We didn't detect any other car so draw a black line with the distance that we use to check for other car
            Debug.DrawRay(transform.position, transform.up * 12, Color.black);
        }

        // No car was detected but we still need to assign out values so lets just return zero
        otherCarPosition = Vector3.zero;
        otherCarRightVector = Vector3.zero;

        return false;
    }

    private void AvoidCars(Vector2 vectorToTarget, out Vector2 newVectorToTarget)
    {
        if (IsCarInFrontOfAICar(out Vector3 otherCarPosition, out Vector3 otherCarRightVector))
        {
            Vector2 avoidanceVector = Vector2.zero;

            // Calculate the reflecting vector if we would hit the other car
            avoidanceVector = Vector2.Reflect((otherCarPosition - transform.position).normalized, otherCarRightVector);

            float distanceToTarget = (targetPosition - transform.position).magnitude;

            // We want to be able to control how much desire the AI has to drive towards paypoint vs avoiding the other cars
            // As we get closer to the waypoint the desire to reach the waypoint increases
            float driveToTargetInfluence = 6.0f / distanceToTarget;

            // Ensure that we limit the value to between 30% and 100% as we always want the AI to desire to reach the waypoint
            driveToTargetInfluence = Mathf.Clamp(driveToTargetInfluence, 0.3f, 1.0f);

            // The desire to avoid the car is simply the inverse to reach te waypoint
            float avoidanceInfluence = 1.0f - driveToTargetInfluence;

            // Reduce jitter a little bit by using a lerp
            avoidanceVectorLerp = Vector2.Lerp(avoidanceVectorLerp, avoidanceVector, Time.fixedDeltaTime * 4);

            // Avoidance vector           
            newVectorToTarget = vectorToTarget * driveToTargetInfluence + avoidanceVectorLerp * avoidanceInfluence;
            newVectorToTarget.Normalize();

            // Draw the vector which indicates the avoidance vector in green
            Debug.DrawRay(transform.position, avoidanceVector * 10, Color.green);

            // Draw the vector that the car will actually take in yellow
            Debug.DrawRay(transform.position, newVectorToTarget * 10, Color.yellow);

            return;
        }

        // We need assign a default value if we didn't hit any cars before we exit the function
        newVectorToTarget = vectorToTarget;
    }


}
