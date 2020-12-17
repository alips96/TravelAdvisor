using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

public class CsvToSo
{
    private static readonly string dataCsvPath = "/TravelAdvisory/Data/covidData.csv";
    private static readonly string usDataCsvPath = "/TravelAdvisory/Data/USCovidData.csv";

    [MenuItem("Utilities/Generate Region Stats")]
    public static void GenerateRegionStats()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + dataCsvPath);

        for (int i = 1; i < allLines.Length; i++)
        {
            if (i > 649 && i < 3924) //skip US
                continue;

            string line = allLines[i].Replace("\"", string.Empty);
            line = line.Replace("*", string.Empty);
            string[] column = line.Split(',');

            Region region = ScriptableObject.CreateInstance<Region>();

            if (column[3] == "Korea" || column[2] == "Bonaire")
            {
                continue;
            }
            if (column[2].CompareTo("") == 0) //if state/region is specified in the dataset
            {
                region.Province_State = string.Empty; //this means the stats are provided only for the  country.
                region.Country_Region = column[3];
                region.Confirmed = Convert.ToInt32(column[7]);
                region.Deaths = Convert.ToInt32(column[8]);
                region.Recovered = Convert.ToInt32(column[9]);
                region.Active = column[10].CompareTo("") == 0 ? 0 : Convert.ToInt32(column[10]);
                region.Combined_Key = column[11].ToLower();
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
                region.Combined_Key = column[11].ToLower() + "," + column[12].ToLower();
                region.Incident_Rate = column[13].CompareTo("") == 0 ? 0f : Convert.ToDouble(column[13]);
                region.Case_Fatality_Ratio = column[14].CompareTo("") == 0 ? 0f : Convert.ToDouble(column[14]);
            }

            AssetDatabase.CreateAsset(region, $"Assets/TravelAdvisory/ScriptableObjects/Regions/Resources/{region.Combined_Key}.asset");
        }

        //Here comes the US
        allLines = File.ReadAllLines(Application.dataPath + usDataCsvPath);

        foreach (string line in allLines.Skip(1))
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
            region.Combined_Key = column[0].ToLower() + ", us";

            AssetDatabase.CreateAsset(region, $"Assets/TravelAdvisory/ScriptableObjects/Regions/Resources/{region.Combined_Key}.asset");
        }

        AssetDatabase.SaveAssets();
    }
}
