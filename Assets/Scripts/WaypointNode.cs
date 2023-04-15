using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNode : MonoBehaviour
{
    [Header("This is the wayoint we are going towards, not yet reached")]
    [SerializeField] public float minimalDistanceToReachWaypoint = 5f;

    [SerializeField] public WaypointNode[] nextWaypointNode;
}
