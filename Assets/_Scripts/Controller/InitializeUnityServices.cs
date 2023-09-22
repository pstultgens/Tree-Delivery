using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Services.Analytics;
using Unity.Services.Core;

public class InitializeUnityServices : MonoBehaviour
{
    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
            Debug.Log("consentIdentifiers: " + consentIdentifiers.Count);
        }
        catch (ConsentCheckException e)
        {
            // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately.
        }
    }    
}