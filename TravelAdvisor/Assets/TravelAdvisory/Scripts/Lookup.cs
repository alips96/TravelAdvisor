using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Lookup : MonoBehaviour
{
    private LocationMaster locationMaster;

    string[] worldLines;
    string[] usLines;

    private string startLocation;

    private void OnEnable()
    {
        SetInitialReferences();

        locationMaster.EventStartingPositionCaptured += SetStartingPointReference;
        locationMaster.EventDestinationCaptured += SetDestinationReference;
        locationMaster.EventDataDownloaded += ProcessCsv;
    }

    private void OnDisable()
    {
        locationMaster.EventStartingPositionCaptured -= SetStartingPointReference;
        locationMaster.EventDestinationCaptured -= SetDestinationReference;
        locationMaster.EventDataDownloaded -= ProcessCsv;
    }

    private void SetInitialReferences()
    {
        locationMaster = GetComponent<LocationMaster>();
    }

    private void SetStartingPointReference(string sPoint)
    {
        startLocation = sPoint;
    }

    private void SetDestinationReference(string endPoint)
    {
        LookupData(endPoint, true);
    }

    private void ProcessCsv()
    {
        worldLines = File.ReadAllLines(Application.dataPath + "/TravelAdvisory/Data/covidData.csv");
        usLines = File.ReadAllLines(Application.dataPath + "/TravelAdvisory/Data/USCovidData.csv");

        StartCoroutine(WaitForStartingPoint());
    }

    private IEnumerator WaitForStartingPoint()
    {
        yield return new WaitUntil(() => !string.IsNullOrEmpty(startLocation));
        LookupData(startLocation, false);
    }

    private void LookupData(string location, bool isDestination)
    {
        if(location[location.Length - 1].CompareTo('S') == 0) //US
        {
            location = location.Replace(" ", "");

            foreach (string line in usLines.Skip(1))
            {
                if (line.Contains(location))
                {
                    ExtractAndSaveUSData(line, isDestination);
                    break;
                }
            }
        }
        else //rest of the world
        {
            for (int i = 1; i < 650 || i > 3923; i++)
            {
                if (worldLines[i].Contains(location))
                {
                    ExtractAndSaveWorldData(worldLines[i], isDestination);
                    break;
                }
            }
        }

        AssetDatabase.SaveAssets();
    }

    private void ExtractAndSaveWorldData(string line, bool isDestination)
    {
        line = line.Replace("\"", string.Empty);
        string[] column = line.Split(',');

        Region region = ScriptableObject.CreateInstance<Region>();

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

        SaveAsset(isDestination, region);
    }

    private void SaveAsset(bool isDestination, Region region)
    {
        if (isDestination)
        {
            AssetDatabase.CreateAsset(region, $"Assets/TravelAdvisory/ScriptableObjects/Regions/Resources/End.asset");
        }
        else
        {
            AssetDatabase.CreateAsset(region, $"Assets/TravelAdvisory/ScriptableObjects/Regions/Resources/Start.asset");
        }
    }

    private void ExtractAndSaveUSData(string line, bool isDestination)
    {
        string[] column = line.Split(',');

        Region region = ScriptableObject.CreateInstance<Region>();

        region.Province_State = column[0];
        region.Country_Region = "US";
        region.Confirmed = Convert.ToInt32(column[5]);
        region.Deaths = Convert.ToInt32(column[6]);
        region.Recovered = column[7].CompareTo("") == 0 ? 0 : (int)float.Parse(column[7]);
        region.Active = (int)float.Parse(column[8]);
        region.Incident_Rate = column[10].CompareTo("") == 0 ? 0f : Convert.ToDouble(column[10]);
        region.Case_Fatality_Ratio = column[13].CompareTo("") == 0 ? 0f : Convert.ToDouble(column[13]);
        region.Combined_Key = column[0] + ", US";

        SaveAsset(isDestination, region);
    }
}