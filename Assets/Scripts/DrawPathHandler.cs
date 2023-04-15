using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;


public class DrawPathHandler : MonoBehaviour
{
    [SerializeField] public Transform transformRootObject;

    private WaypointNode[] waypointNodes;



    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (transformRootObject == null)
        {
            return;
        }

        //Get all Waypoints
        waypointNodes = transformRootObject.GetComponentsInChildren<WaypointNode>();

        //Iterate the list
        foreach (WaypointNode waypoint in waypointNodes)
        {

            foreach (WaypointNode nextWayPoint in waypoint.nextWaypointNode)
            {
                if (nextWayPoint != null)
                {
                    Gizmos.DrawLine(waypoint.transform.position, nextWayPoint.transform.position);
                }

            }

        }
    }

}
