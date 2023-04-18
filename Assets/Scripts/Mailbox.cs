using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Mailbox : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public int correctValue;

    [Header("Minimap Icons")]
    [SerializeField] public GameObject minimapNode;
    [SerializeField] public GameObject minimapEdgeLeft;
    [SerializeField] public GameObject minimapEdgeRight;

    [Header("Edge obstacles")]
    [SerializeField] public GameObject edgeLeftClosed;
    [SerializeField] public GameObject edgeRightClosed;

    public bool isComplete;

    public void Complete()
    {
        isComplete = true;
        OpenToNextNode();
    }

    private void OpenToNextNode()
    {
        if (edgeLeftClosed != null)
        {
            Destroy(edgeLeftClosed);
        }

        if(edgeRightClosed != null)
        {
            Destroy(edgeRightClosed);
        }
        
    }
}
