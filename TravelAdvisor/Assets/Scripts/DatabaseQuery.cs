using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseQuery : MonoBehaviour
{
    private LocationMaster locationMaster;

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
        Debug.Log("start: " + sPoint);
    }

    private void EndPoint(string endPoint)
    {
        Debug.Log("End: " + endPoint);
    }
}
