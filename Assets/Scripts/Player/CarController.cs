using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Car settings")]
    [SerializeField] public float driftFactor = 0.95f;
    [SerializeField] public float accelerationFactor = 30f;
    [SerializeField] public float turnFactor = 3.5f;
    [SerializeField] public float maxSpeed = 20f;

    [Header("Powerup settings")]
    [SerializeField] public float boostSpeed = 30f;
    [SerializeField] public float boostTime = 2f;

    [Header("Trap settings")]
    [SerializeField] public float oilTrapDriftFactor = 1f;
    [SerializeField] public float oilTrapTurnFactor = 10f;
    [SerializeField] public float oilTrapTime = 2f;

    [SerializeField] public float spikeTrapSpeed = 6f;
    [SerializeField] public float spikeTrapTurnFactor = 2f;
    [SerializeField] public float spikeTrapTime = 2f;

    [Header("Sprites")]
    [SerializeField] public SpriteRenderer carSpriteRenderer;
    [SerializeField] public SpriteRenderer carShadowSpriteRenderer;

    private float accelerationInput = 0;
    private float steeringInput = 0;
    private float rotationAngle = 0;
    private float velocityVsUp = 0;

    private float originalDriftFactor;
    private float originalTurnFactor;
    private float originalMaxSpeed;

    private bool inBoostMode;
    private bool isAI;

    public bool inOilTrapMode;
    public bool inSpikeTrapMode;

    private Rigidbody2D carRigidbody;

    private void Awake()
    {
        carRigidbody = GetComponent<Rigidbody2D>();
        isAI = GetComponent<CarIAHandler>() != null;
    }

    void Start()
    {
        rotationAngle = transform.rotation.eulerAngles.z;

        originalDriftFactor = driftFactor;
        originalTurnFactor = turnFactor;
        originalMaxSpeed = maxSpeed;
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();
        KillOrthogonalVelocity();
        ApplySteering();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isAI)
        {
            return;
        }

        // Shake Camera
        float relativeVelocity = other.relativeVelocity.magnitude;
        float intensity = relativeVelocity * 0.4f;
        Debug.Log("Shake Camera with intensity: " + intensity);
        CinemachineShake.Instance.ShakeCamera(intensity, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Boost") && !inBoostMode)
        {
            inBoostMode = true;
            StartCoroutine(BoostCoroutine());
        }

        if (other.tag.Equals("OilTrap") && !inOilTrapMode)
        {
            inOilTrapMode = true;
            StartCoroutine(OilTrapCoroutine());
        }

        if (other.tag.Equals("SpikeTrap") && !inSpikeTrapMode)
        {
            inSpikeTrapMode = true;
            StartCoroutine(SpikeTrapCoroutine());
        }
    }

    private IEnumerator BoostCoroutine()
    {        
        maxSpeed = boostSpeed;
        yield return new WaitForSeconds(boostTime);
        maxSpeed = originalMaxSpeed;
        inBoostMode = false;
    }

    private IEnumerator OilTrapCoroutine()
    {        
        driftFactor = oilTrapDriftFactor;
        turnFactor = oilTrapTurnFactor;
        yield return new WaitForSeconds(oilTrapTime);
        driftFactor = originalDriftFactor;
        turnFactor = originalTurnFactor;
        inOilTrapMode = false;
    }

    private IEnumerator SpikeTrapCoroutine()
    {        
        maxSpeed = spikeTrapSpeed;
        turnFactor = spikeTrapTurnFactor;
        yield return new WaitForSeconds(spikeTrapTime);
        maxSpeed = originalMaxSpeed;
        turnFactor = originalTurnFactor;
        inSpikeTrapMode = false;
    }

    private void ApplyEngineForce()
    {
        // Drag if max speed has changed into less
        if (originalMaxSpeed > maxSpeed)
        {
            carRigidbody.drag = Mathf.Lerp(carRigidbody.drag, 3f, Time.fixedDeltaTime * 3);
        }

        // Calculate how much "forward" we are going in terms of the direction of our velocity
        velocityVsUp = Vector2.Dot(transform.up, carRigidbody.velocity);

        // Limit so we cant go faster than the max speed in the "forward" direction
        if (velocityVsUp > maxSpeed && accelerationInput > 0)
        {
            return;
        }

        // Limit so we cant go faster than the 50% of max speed in the "reverse" direction
        if (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0)
        {
            return;
        }

        // Limit so we cant go faster in any direction while accelerating
        if (carRigidbody.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
        {
            return;
        }

        // Apply drag if there is no accelerationInput so the car stops when the player lets go of the accelerator
        if (accelerationInput == 0)
        {
            if (inOilTrapMode)
            {
                carRigidbody.drag = Mathf.Lerp(carRigidbody.drag, 1f, Time.fixedDeltaTime * 3);
            }
            else
            {
                carRigidbody.drag = Mathf.Lerp(carRigidbody.drag, 3f, Time.fixedDeltaTime * 3);
            }
        }
        else
        {
            carRigidbody.drag = 0;
        }

        // Create a force for the engine
        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;

        // Apply force and pushes the car forward
        carRigidbody.AddForce(engineForceVector, ForceMode2D.Force);
    }

    private void ApplySteering()
    {
        // Limit the cars ability to turn when moving slowly
        float minSpeedBeforeAllowTurningFactor = (carRigidbody.velocity.magnitude / 8);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        // Update the rotation angle based on input
        rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;

        // Apply steering by rotating the car object
        carRigidbody.MoveRotation(rotationAngle);
    }

    private void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody.velocity, transform.right);

        carRigidbody.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    private float GetLateralVelocity()
    {
        // Returns how fast the car is moving sideways
        return Vector2.Dot(transform.right, carRigidbody.velocity);
    }

    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        // Check if we ware moving forward and if the player is hitting the brakes
        // In that case the tires should screech
        if (velocityVsUp > 0 && accelerationInput < 0)
        {
            isBraking = true;
            return true;
        }

        // If we have a lot of side movement then the tires should be screeching
        if (Mathf.Abs(GetLateralVelocity()) > 4f)
        {
            return true;
        }

        return false;
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }

    public void SetSteering(float x)
    {
        steeringInput = x;
    }

    public void SetAcceleration(float y)
    {
        accelerationInput = y;
    }

    public float GetVelocityMagnitude()
    {
        return carRigidbody.velocity.magnitude;
    }

}
