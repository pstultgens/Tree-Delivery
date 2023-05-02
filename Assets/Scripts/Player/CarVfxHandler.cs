using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarVfxHandler : MonoBehaviour
{
    

    [SerializeField] public ParticleSystem collisionVFX;
    [SerializeField] public ParticleSystem pickupPackageVFX;

    private float collisionVFXEmissionRate = 0;

    void Update()
    {
        var emission = collisionVFX.emission;

        // Reduce the particles over time
        collisionVFXEmissionRate = Mathf.Lerp(collisionVFXEmissionRate, 0, Time.deltaTime * 10f);
        emission.rateOverTime = collisionVFXEmissionRate;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayCollisionVFX(other);
    }

    private void PlayCollisionVFX(Collision2D other)
    {
        ContactPoint2D contact = other.contacts[0];
        collisionVFX.transform.position = contact.point;
        float relativeVelocity = other.relativeVelocity.magnitude;
        collisionVFXEmissionRate = relativeVelocity * 5f;
    }

    public void PlayPickupPackageVFX(Vector2 location)
    {
        pickupPackageVFX.transform.position = location;
        pickupPackageVFX.Play();
    }

    public void PlayDropPackageVFX(Vector2 location)
    {
        pickupPackageVFX.transform.position = location;
        pickupPackageVFX.Play();
    }
}
