using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionParticleHandler : MonoBehaviour
{
    private float particleEmissionRate = 0;

    [SerializeField] public ParticleSystem collisionParticleSystem;

    // Update is called once per frame
    void Update()
    {
        var emission = collisionParticleSystem.emission;

        // Reduce the particles over time
        particleEmissionRate = Mathf.Lerp(particleEmissionRate, 0, Time.deltaTime * 10f);
        emission.rateOverTime = particleEmissionRate;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ContactPoint2D contact = other.contacts[0];
        collisionParticleSystem.transform.position = contact.point;

        float relativeVelocity = other.relativeVelocity.magnitude;
        particleEmissionRate = relativeVelocity * 5f;

        Debug.Log("Emission: " + particleEmissionRate);
    }
}
