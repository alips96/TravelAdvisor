using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationMaster : MonoBehaviour
{
    public delegate void GeneralEventHandler(string someString);

    public event GeneralEventHandler EventStartingPositionCaptured;
    public event GeneralEventHandler EventDestinationCaptured;

    public void CallEventStartingPositionCaptured(string startPos)
    {
        EventStartingPositionCaptured.Invoke(startPos);
    }

    public void CallEventDestinationCaptured(string endPos)
    {
        EventDestinationCaptured.Invoke(endPos);
    }
}
