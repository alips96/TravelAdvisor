using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

public class CsvToSo
{
    private static string dataCsvPath = "/Editor/Data/covidData.csv";

    [MenuItem("Utilities/Generate Region Stats")]
    public static void GenerateRegionStats()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + dataCsvPath);

        foreach (string line in allLines.Skip(1))
        {
            string updatedLine = line.Replace("\"", string.Empty);
            updatedLine = updatedLine.Replace("*", string.Empty);
            string[] column = updatedLine.Split(',');

            Region region = ScriptableObject.CreateInstance<Region>();

            if (column[3] == "US")
                continue;

            if (column[3] == "Korea" || column[2] == "Bonaire")
            {
                continue;
            }
            if (column[2].CompareTo("") == 0) //if state/region is specified in the dataset
            {
                region.Province_State = "n/a"; //this means the stats are provided only for the  country.
                region.Country_Region = column[3];
                region.Confirmed = column[7].CompareTo("") == 0 ? 0 : Convert.ToInt32(column[7]);
                region.Deaths = column[8].CompareTo("") == 0 ? 0 : Convert.ToInt32(column[8]);
                region.Recovered = column[9].CompareTo("") == 0 ? 0 : Convert.ToInt32(column[9]);
                region.Active = column[10].CompareTo("") == 0 ? 0 : Convert.ToInt32(column[10]);
                region.Combined_Key = column[11];
                region.Incident_Rate = column[12].CompareTo("") == 0 ? 0f : Convert.ToDouble(column[12]);
                region.Case_Fatality_Ratio = column[13].CompareTo("") == 0 ? 0f : Convert.ToDouble(column[13]);
            }
            else
            {
                region.Province_State = column[2];
                region.Country_Region = column[3];
                region.Confirmed = column[7].CompareTo("") == 0 ? 0 : Convert.ToInt32(column[7]);
                region.Deaths = column[8].CompareTo("") == 0 ? 0 : Convert.ToInt32(column[8]);
                region.Recovered = column[9].CompareTo("") == 0 ? 0 : Convert.ToInt32(column[9]);
                region.Active = column[10].CompareTo("") == 0 ? 0 : Convert.ToInt32(column[10]);
                region.Combined_Key = column[11] + ", " + column[12];
                region.Incident_Rate = column[13].CompareTo("") == 0 ? 0f : Convert.ToDouble(column[13]);
                region.Case_Fatality_Ratio = column[14].CompareTo("") == 0 ? 0f : Convert.ToDouble(column[14]);
            }
            

            AssetDatabase.CreateAsset(region, $"Assets/ScriptableObjects/Regions/{region.Combined_Key}.asset");
        }

        AssetDatabase.SaveAssets();
    }
}
