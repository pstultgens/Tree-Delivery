using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNode : MonoBehaviour
{
    // Max speed allowed when passing this waypoint
    [Header("Speed set once we reach the waypoint")]
    [SerializeField] public float maxSpeed = 0;

    [Header("This is the wayoint we are going towards, not yet reached")]
    [SerializeField] public float minimalDistanceToReachWaypoint = 5f;

    [SerializeField] public WaypointNode[] nextWaypointNode;
}
