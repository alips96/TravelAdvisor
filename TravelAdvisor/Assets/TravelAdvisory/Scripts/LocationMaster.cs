using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationMaster : MonoBehaviour
{
    public delegate void PositionEventHandler(string someString);
    public event PositionEventHandler EventStartingPositionCaptured;
    public event PositionEventHandler EventDestinationCaptured;

    public delegate void DataEventHandler();
    public event DataEventHandler EventDataDownloaded;

    public delegate void AnalyzerEventHandler(List<string> list);
    public event AnalyzerEventHandler EventAnalyzeData;

    public Region StartingPoint { get; set; }
    public Region Destination { get; set; }

    public void CallEventAnalyzeData(List<string> list)
    {
        EventAnalyzeData.Invoke(list);
    }

    public void CallEventStartingPositionCaptured(string startPos)
    {
        EventStartingPositionCaptured.Invoke(startPos);
    }

    public void CallEventDestinationCaptured(string endPos)
    {
        EventDestinationCaptured.Invoke(endPos);
    }

    public void CallEventDataDownloaded()
    {
        EventDataDownloaded.Invoke();
    }
}
