using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShadowCasterController : MonoBehaviour
{

    void Awake()
    {
#if UNITY_WEBGL
        GetComponent<ShadowCaster2D>().enabled = false;
#endif
    }

}
