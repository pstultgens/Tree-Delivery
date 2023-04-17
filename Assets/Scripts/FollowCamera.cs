using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] public GameObject thingToFollow;

    void LateUpdate()
    {
        if (thingToFollow != null)
        {
            transform.position = thingToFollow.transform.position + new Vector3(0, 0, -10);
        } else
        {
            Debug.Log("Camera thingToFollow is Empty!");
        }
    }
}
