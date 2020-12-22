using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Result : MonoBehaviour
{
    private Region startingPoint;
    private Region destination;

    //Starting Point
    [SerializeField] private TMP_Text SState;
    [SerializeField] private TMP_Text SCountry;
    [SerializeField] private TMP_Text SConfirmed;
    [SerializeField] private TMP_Text SDeaths;
    [SerializeField] private TMP_Text SRecovered;
    [SerializeField] private TMP_Text SActive;
    [SerializeField] private TMP_Text SIncidentRate;
    [SerializeField] private TMP_Text SCaseFatalityRatio;
    [SerializeField] private TMP_Text SStatus;

    //Destination
    [SerializeField] private TMP_Text DState;
    [SerializeField] private TMP_Text DCountry;
    [SerializeField] private TMP_Text DConfirmed;
    [SerializeField] private TMP_Text DDeaths;
    [SerializeField] private TMP_Text DRecovered;
    [SerializeField] private TMP_Text DActive;
    [SerializeField] private TMP_Text DIncidentRate;
    [SerializeField] private TMP_Text DCaseFatalityRatio;
    [SerializeField] private TMP_Text DStatus;

    [SerializeField] private TMP_Text OverallStatus;

    Dictionary<string, Condition> StateConditionsDic;

    private void Start()
    {
        SetInitialReferences();
        SetStatsUI();
        AssignColorsToStats();
    }

    private void AssignColorsToStats()
    {
        foreach (KeyValuePair<string, Condition> item in StateConditionsDic)
        {
            AnalyzeAndAssignAColor(item);
        }
    }

    private void AnalyzeAndAssignAColor(KeyValuePair<string, Condition> item)
    {
        switch (item.Key)
        {
            case "SIR":
                AnalyzeIncidentRate(false);
                break;

            case "SCF":
                AnalyzeCaseFatalityRatio(false);
                break;

            case "DIR":
                AnalyzeIncidentRate(true);
                break;

            case "DCF":
                AnalyzeCaseFatalityRatio(true);
                break;
        }
    }

    private void AnalyzeCaseFatalityRatio(bool isDestination)
    {
        throw new NotImplementedException();
    }

    private void AnalyzeIncidentRate(bool isDestination)
    {
        throw new NotImplementedException();
    }

    private void SetStatsUI()
    {
        //StartingPoint
        SState.text = startingPoint.Province_State;
        SCountry.text = startingPoint.Country_Region;
        SConfirmed.text = startingPoint.Confirmed.ToString();
        SDeaths.text = startingPoint.Deaths.ToString();
        SRecovered.text = startingPoint.Recovered.ToString();
        SActive.text = startingPoint.Active.ToString();
        SIncidentRate.text = Math.Round(startingPoint.Incident_Rate, 2).ToString();
        SCaseFatalityRatio.text = Math.Round(startingPoint.Case_Fatality_Ratio, 3).ToString();

        //Destination
        DState.text = destination.Province_State;
        DCountry.text = destination.Country_Region;
        DConfirmed.text = destination.Confirmed.ToString();
        DDeaths.text = destination.Deaths.ToString();
        DRecovered.text = destination.Recovered.ToString();
        DActive.text = destination.Active.ToString();
        DIncidentRate.text = Math.Round(destination.Incident_Rate, 2).ToString();
        DCaseFatalityRatio.text = Math.Round(destination.Case_Fatality_Ratio, 3).ToString();
    }

    private void SetInitialReferences()
    {
        startingPoint = Resources.Load<Region>("Start");
        destination = Resources.Load<Region>("End");

        StateConditionsDic = new Dictionary<string, Condition>()
        {
            {"SIR", Condition.Green },
            {"SCF", Condition.Green },
            {"DIR", Condition.Green },
            {"DCF", Condition.Green }
        };
    }

    private enum Condition : byte
    {
        Green = 2,
        Yellow = 3,
        Orange = 4,
        Red = 5
    }
}
