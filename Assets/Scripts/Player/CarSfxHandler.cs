using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CarSfxHandler : MonoBehaviour
{
    [Header("Audio Mixers")]
    [SerializeField] public AudioMixer audioMixer;

    [Header("Audio sources")]
    [SerializeField] public AudioSource tireScreechingAudioSource;
    [SerializeField] public AudioSource engineAudioSource;
    [SerializeField] public AudioSource carHitAudioSource;
    [SerializeField] public AudioSource boosterAudioSource;
    [SerializeField] public AudioSource spikeTrapAudioSource;
    [SerializeField] public AudioSource oilTrapAudioSource;

    private float enginePitch = 0.5f;
    private float tiresScreechingPitch = 0.5f;

    CarController carController;

    private void Awake()
    {
        carController = GetComponent<CarController>();
    }

    void Update()
    {
        UpdateEngineSFX();
        UpdateTiresScreechingSFX();
    }

    private void UpdateEngineSFX()
    {
        // Handle engine SFX
        float velocityMagnitude = carController.GetVelocityMagnitude();

        // Increase the engine volume as the car goes faster
        float desiredEngineVolume = velocityMagnitude * 0.05f;

        // But keep the minimum level so it plays even if the car is idle
        desiredEngineVolume = Mathf.Clamp(desiredEngineVolume, 0.2f, 1.0f);

        engineAudioSource.volume = Mathf.Lerp(engineAudioSource.volume, desiredEngineVolume, Time.deltaTime * 10);

        // To add more variation to the engine sound we also change the pitch
        enginePitch = velocityMagnitude * 0.2f;
        enginePitch = Mathf.Clamp(enginePitch, 0.5f, 2f);
        engineAudioSource.pitch = Mathf.Lerp(engineAudioSource.pitch, enginePitch, Time.deltaTime * 1.5f);
    }

    private void UpdateTiresScreechingSFX()
    {
        // Handle tire screeching SFX
        if (carController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            // If the car is braking we want the tire screech to be louder and also change the pitch
            if (isBraking)
            {
                tireScreechingAudioSource.volume = Mathf.Lerp(tireScreechingAudioSource.volume, 1f, Time.deltaTime * 10);
                tiresScreechingPitch = Mathf.Lerp(tiresScreechingPitch, 0.5f, Time.deltaTime * 10);
            }
            else
            {
                // If we are not braking we still want to play this screech sound if the player is drifting
                tireScreechingAudioSource.volume = Mathf.Abs(lateralVelocity) * 0.5f;
                tiresScreechingPitch = Mathf.Abs(lateralVelocity) * 0.1f;
            }
        }
        else
        {
            // Fade out the tire screech SFX if we are not screeching
            tireScreechingAudioSource.volume = Mathf.Lerp(tireScreechingAudioSource.volume, 0, Time.deltaTime * 10);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Get the relative velocity of the collision
        float relativeVelocity = other.relativeVelocity.magnitude;
        float volume = relativeVelocity * 0.1f;

        carHitAudioSource.pitch = Random.Range(0.95f, 1.05f);
        carHitAudioSource.volume = volume;

        if (!carHitAudioSource.isPlaying)
        {
            carHitAudioSource.Play();
        }
    }
       

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Boost") && !boosterAudioSource.isPlaying)
        {
            PlayBoosterSFX();
        }

        if (other.tag.Equals("SpikeTrap") && !spikeTrapAudioSource.isPlaying)
        {
            PlaySpikeTrapSFX();
        }

        if (other.tag.Equals("OilTrap") && !oilTrapAudioSource.isPlaying)
        {
            PlayOilTrapSFX();
        }
    }

    public void PlaySpikeTrapSFX()
    {
        spikeTrapAudioSource.Play();
    }

    public void PlayOilTrapSFX()
    {
        oilTrapAudioSource.Play();
    }

    public void PlayBoosterSFX()
    {
        boosterAudioSource.Play();
    }
}
