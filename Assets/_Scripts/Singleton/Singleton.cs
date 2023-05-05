using System;
using UnityEngine;

public abstract class Singleton : MonoBehaviour
{
    // Singleton Pattern
    private void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1) // GetType() gets the type from this instance. Reusable in other classes without to specify the Type.
        {
            gameObject.SetActive(false); // Important line, else there is a chance to instantiate 2 before 1 gets destroyed.
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
