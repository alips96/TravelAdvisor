using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Region", menuName = "Assets/ScriptibleObjects/New Region")]
public class Region : ScriptableObject
{
    public string Province_State;
    public string Country_Region;
    public int Confirmed;
    public int Deaths;
    public int Recovered;
    public int Active;
    public string Combined_Key;
    public double Incident_Rate;
    public double Case_Fatality_Ratio;
}
