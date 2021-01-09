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
    private Analyzer analyzerScript;

    string[] worldLines;
    string[] usLines;
    private List<string> worldList;
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
        analyzerScript = GetComponent<Analyzer>();
    }

    private void SetStartingPointReference(string sPoint)
    {
        startLocation = sPoint;
    }

    private void SetDestinationReference(string endPoint)
    {
        LookupData(endPoint, true);
    }

    private void ProcessCsv() //remember to get it back to private after testing
    {
        worldLines = File.ReadAllLines(Application.dataPath + "/TravelAdvisory/Data/covidData.csv");
        usLines = File.ReadAllLines(Application.dataPath + "/TravelAdvisory/Data/USCovidData.csv");

        // call event analyse data

        worldList = new List<string>();
        List<string> usList;

        for (int i = 1; i < worldLines.Length; i++)
        {
            if (i > 650 && i < 3926) //skip US
                continue;

            worldList.Add(worldLines[i]);
        }
        usList = usLines.Skip(1).ToList();

        worldList.AddRange(usList);
        locationMaster.CallEventAnalyzeData(worldList);

        StartCoroutine(WaitForStartingPoint());
    }

    private IEnumerator WaitForStartingPoint()
    {
        yield return new WaitUntil(() => !string.IsNullOrEmpty(startLocation));
        LookupData(startLocation, false);
    }

    private void LookupData(string location, bool isDestination)
    {
        if (location[location.Length - 1].CompareTo('S') == 0) //US
        {
            location = location.Replace(" ", "");

            for (int i = 702; i < worldList.Count; i++)
            {
                if (worldList[i].Contains(location))
                {
                    ExtractAndSaveUSData(worldList[i], isDestination, analyzerScript.GetCorrespondingIndex(i));
                    break;
                }
            }
        }
        else //rest of the world
        {
            for (int i = 0; i < 702; i++)
            {
                if (worldList[i].Contains(location))
                {
                    ExtractAndSaveWorldData(worldList[i], isDestination, analyzerScript.GetCorrespondingIndex(i));
                    break;
                }
            }
        }

        AssetDatabase.SaveAssets();
    }

    private void ExtractAndSaveWorldData(string line, bool isDestination, int statusIndex)
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

        region.statusIndex = statusIndex;

        SaveAsset(isDestination, region);
    }

    private void SaveAsset(bool isDestination, Region region)
    {
        if (isDestination)
        {
            AssetDatabase.CreateAsset(region, $"Assets/TravelAdvisory/Resources/ScriptableObjects/Regions/End.asset");
        }
        else
        {
            AssetDatabase.CreateAsset(region, $"Assets/TravelAdvisory/Resources/ScriptableObjects/Regions/Start.asset");
        }
    }

    private void ExtractAndSaveUSData(string line, bool isDestination, int statusIndex)
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
        region.statusIndex = statusIndex;

        SaveAsset(isDestination, region);
    }
}