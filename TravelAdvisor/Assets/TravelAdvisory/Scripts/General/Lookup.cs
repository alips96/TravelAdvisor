using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Lookup : MonoBehaviour
{
    private LocationMaster locationMaster;

    private List<string> worldList = new List<string>();

    private CovidData data;

    private void OnEnable()
    {
        SetInitialReferences();

        locationMaster.EventStartingPositionCaptured += SetStartingPointReference;
        locationMaster.EventDestinationCaptured += SetDestinationReference;
        locationMaster.EventDataAnalyzed += InitializeEssentialData;
    }

    private void OnDisable()
    {
        locationMaster.EventStartingPositionCaptured -= SetStartingPointReference;
        locationMaster.EventDestinationCaptured -= SetDestinationReference;
        locationMaster.EventDataAnalyzed -= InitializeEssentialData;
    }

    private void InitializeEssentialData()
    {
        data = SaveSystem.LoadData();

        worldList = data.worldList;
    }

    private void SetInitialReferences()
    {
        locationMaster = GetComponent<LocationMaster>();
    }

    private void SetStartingPointReference(string sPoint)
    {
        StartCoroutine(LookupData(sPoint, false));
    }

    private void SetDestinationReference(string endPoint)
    {
        StartCoroutine(LookupData(endPoint, true));
    }

    private IEnumerator LookupData(string location, bool isDestination)
    {
        yield return new WaitUntil(() => worldList.Count > 0);

        if (location[location.Length - 1].CompareTo('S') == 0) //US
        {
            location = NormalizeLocation(location);

            for (int i = worldList.Count - 58; i < worldList.Count; i++) //58 US States
            {
                if (worldList[i].Contains(location))
                {
                    ExtractAndSaveUSData(worldList[i], isDestination, data.GetCorrespondingSituationIndex(i));
                    break;
                }
            }
        }
        else //rest of the world
        {
            int usStartIndex = worldList.Count - 58; //58 US States

            for (int i = 0; i < usStartIndex; i++)
            {
                if (worldList[i].Contains(location))
                {
                    ExtractAndSaveWorldData(worldList[i], isDestination, data.GetCorrespondingSituationIndex(i));
                    break;
                }
            }
        }
    }

    private static string NormalizeLocation(string location)
    {
        int removeIndex = location.Length - 3;

        StringBuilder builder = new StringBuilder(location);
        builder.Replace(" ", string.Empty, removeIndex, 1);
        location = builder.ToString();

        return location;
    }

    private void ExtractAndSaveWorldData(string line, bool isDestination, int statusIndex)
    {
        line = line.Replace("\"", string.Empty);
        string[] column = line.Split(',');

        Region region = new Region();

        if (column[2].CompareTo("") == 0) //if state/region is not specified in the dataset
        {
            region.Province_State = string.Empty; //this means the stats are provided only for the country.
            region.Country_Region = column[3];
            region.Confirmed = Convert.ToInt32(column[7]);
            region.Deaths = Convert.ToInt32(column[8]);
            region.Recovered = Convert.ToInt32(column[9]);
            region.Active = column[10].CompareTo("") == 0 ? 0 : Convert.ToInt32(column[10]);
            region.Combined_Key = column[11];
            region.Incident_Rate = column[12].CompareTo("") == 0 ? 0f : Convert.ToDouble(column[12]);
            region.Case_Fatality_Ratio = column[13].CompareTo("") == 0 ? 0f : Convert.ToDouble(column[13]);
        }
        else
        {
            region.Province_State = column[2];
            region.Country_Region = column[3];
            region.Confirmed = Convert.ToInt32(column[7]);
            region.Deaths = Convert.ToInt32(column[8]);
            region.Recovered = Convert.ToInt32(column[9]);
            region.Active = column[10].CompareTo("") == 0 ? 0 : Convert.ToInt32(column[10]);
            region.Combined_Key = column[11] + "," + column[12];
            region.Incident_Rate = column[13].CompareTo("") == 0 ? 0f : Convert.ToDouble(column[13]);
            region.Case_Fatality_Ratio = column[14].CompareTo("") == 0 ? 0f : Convert.ToDouble(column[14]);
        }

        region.StatusIndex = statusIndex;

        SaveAsset(isDestination, region);
    }

    private void SaveAsset(bool isDestination, Region region)
    {
        if (isDestination)
        {
            locationMaster.Destination = region;
        }
        else
        {
            locationMaster.StartingPoint = region;
        }
    }

    private void ExtractAndSaveUSData(string line, bool isDestination, int statusIndex)
    {
        string[] column = line.Split(',');

        Region region = new Region
        {
            Province_State = column[0],
            Country_Region = "US",
            Confirmed = Convert.ToInt32(column[5]),
            Deaths = Convert.ToInt32(column[6]),
            Recovered = column[7].CompareTo("") == 0 ? 0 : (int)float.Parse(column[7]),
            Active = (int)float.Parse(column[8]),
            Incident_Rate = column[10].CompareTo("") == 0 ? 0f : Convert.ToDouble(column[10]),
            Case_Fatality_Ratio = column[13].CompareTo("") == 0 ? 0f : Convert.ToDouble(column[13]),
            Combined_Key = column[0] + ", US",
            StatusIndex = statusIndex
        };

        SaveAsset(isDestination, region);
    }
}