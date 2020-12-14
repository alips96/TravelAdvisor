using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyzeData : MonoBehaviour
{
    private LocationMaster locationMaster;
    private Region startingPointData;
    private Region destinationData;

    private void OnEnable()
    {
        SetInitialReferences();

        locationMaster.EventStartingPositionCaptured += SetStartingPointReference;
        locationMaster.EventDestinationCaptured += SetDestinationReference;
    }

    private void OnDisable()
    {
        locationMaster.EventStartingPositionCaptured -= SetStartingPointReference;
        locationMaster.EventDestinationCaptured -= SetDestinationReference;
    }

    private void SetInitialReferences()
    {
        locationMaster = GetComponent<LocationMaster>();
    }

    private void SetStartingPointReference(string sPoint)
    {
        startingPointData = Resources.Load<Region>(sPoint);

        if (startingPointData == null)
            Debug.LogError("Could not find the starting point reference");
    }

    private void SetDestinationReference(string endPoint)
    {
        destinationData = Resources.Load<Region>(endPoint);

        if (destinationData == null)
            Debug.LogError("Could not find the destination reference");
    }
}
