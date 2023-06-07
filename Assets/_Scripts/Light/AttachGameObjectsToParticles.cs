using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(ParticleSystem))]
public class AttachGameObjectsToParticles : MonoBehaviour
{
    public GameObject prefab;

    private ParticleSystem myParticleSystem;
    private List<GameObject> instances = new List<GameObject>();
    private ParticleSystem.Particle[] particles;

    void Start()
    {
        myParticleSystem = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[myParticleSystem.main.maxParticles];
    }

    //void LateUpdate()
    //{
    //    int particleCount = particleSystem.GetParticles(particles);

    //    while (instance.Count < particleCount)
    //    {
    //        instance.Add(Instantiate(prefab, particleSystem.transform));
    //    }

    //    bool worldSpace = (particleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World);
    //    for (int i = 0; i < instance.Count; i++)
    //    {


    //        Vector2 particleSize = new Vector2(particles[i].startSize, particles[i].startSize);
    //        instance[i].transform.localScale = particleSize;

    //        if (i < count)
    //        {

    //            if (worldSpace)
    //            {
    //                instance[i].transform.position = particles[i].position;
    //            }
    //            else
    //            {
    //                instance[i].transform.localPosition = particles[i].position;
    //            }

    //            instance[i].SetActive(true);
    //        }
    //        else
    //        {
    //            instance[i].SetActive(false);
    //        }
    //    }
    //}


    private void LateUpdate()
    {
        int particleCount = myParticleSystem.GetParticles(particles);

        while (instances.Count < particleCount)
        {
            instances.Add(Instantiate(prefab, myParticleSystem.transform.position, Quaternion.identity));
        }

        bool worldSpace = (myParticleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World);
        for (int i = 0; i < instances.Count; i++)
        {
            ParticleSystem.Particle particle = particles[i];

            float startSize = particle.startSize / 6f;
            instances[i].transform.localScale = new Vector3(startSize, startSize, startSize);

            if (i < particleCount)
            {
                if (worldSpace)
                {
                    instances[i].transform.position = particles[i].position;
                }
                else
                {
                    instances[i].transform.localPosition = particles[i].position;
                }

                instances[i].transform.localScale = instances[i].transform.localScale * particles[i].remainingLifetime;
                instances[i].transform.localRotation = Quaternion.AngleAxis(particles[i].rotation, Vector3.forward);

                instances[i].SetActive(true);
            }
            else
            {
                instances[i].SetActive(false);
            }
        }
    }
}
