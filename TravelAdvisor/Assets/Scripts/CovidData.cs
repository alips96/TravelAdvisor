using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CovidData
{
    public string Province_State { get; set; }
    public string Country_Region { get; set; }
    public int Confirmed { get; set; }
    public int Deaths { get; set; }
    public int Recovered { get; set; }
    public int Active { get; set; }
    public string Combined_Key { get; set; }
    public double Incident_Rate { get; set; }
    public double Case_Fatality_Ratio { get; set; }

    internal static CovidData ParseRow(string row)
    {
        string[] column = TrimRow(ref row);

        return new CovidData()
        {
            Province_State = column[2],
            Country_Region = column[3],
            Confirmed = Convert.ToInt32(column[7]),
            Deaths = Convert.ToInt32(column[8]),
            Recovered = Convert.ToInt32(column[9]),
            Active = Convert.ToInt32(column[10]),
            Combined_Key = column[11],
            Incident_Rate = Convert.ToDouble(column[12]),
            Case_Fatality_Ratio = Convert.ToDouble(column[13])
        };
    }

    private static string[] TrimRow(ref string row)
    {
        row = row.Trim(new char[] { '"' });
        string[] column = row.Split(',');
        return column;
    }

}
