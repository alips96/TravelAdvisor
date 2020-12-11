using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseQuery : MonoBehaviour
{
    private LocationMaster locationMaster;
    private Region startingPointData;
    private Region destinationData;

    private void OnEnable()
    {
        SetInitialReferences();

        locationMaster.EventStartingPositionCaptured += StartingPoint;
        locationMaster.EventDestinationCaptured += EndPoint;
    }

    private void OnDisable()
    {
        locationMaster.EventStartingPositionCaptured -= StartingPoint;
        locationMaster.EventDestinationCaptured -= EndPoint;
    }

    private void SetInitialReferences()
    {
        locationMaster = GetComponent<LocationMaster>();
    }

    private void StartingPoint(string sPoint)
    {
        startingPointData = Resources.Load<Region>(sPoint);
    }

    private void EndPoint(string endPoint)
    {
        destinationData = Resources.Load<Region>(endPoint);
    }
}
