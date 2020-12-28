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
    private Condition[] StateConditionsArr;

    private Condition startingPointStatus;
    private Condition destinationStatus;

    private readonly Dictionary<int, string> ColorsToStatusDic = new Dictionary<int, string>()
    {
        {5, "Very High"},
        {4,"High"},
        {3, "Moderate" },
        {2, "Low" }
    };

    private string capturedString = "";

    public void ShowResults() //Called by go button
    {
        if (destination != null)
        {
            capturedString = destination.Combined_Key;
        }

        SetInitialReferences();

        if(capturedString.CompareTo(destination.Combined_Key) != 0)
        {
            SetStatsUI();
            AssignColorsToStats();
            AnalyzeStatus();
            CalculateOverallRisk();
        }
    }

    private void CalculateOverallRisk()
    {
        int difference = Mathf.Abs(startingPointStatus.Code - destinationStatus.Code);

        switch (difference)
        {
            case 2:
                if(destinationStatus.Code > startingPointStatus.Code)
                {
                    if(destinationStatus.Code == 5)
                    {
                        AssignOverallStatus(new Color(1.0f, 0.64f, 0.0f), "High");
                    }
                    else
                    {
                        AssignOverallStatus(Color.yellow, "Moderate");
                    }
                }
                else
                {
                    if(destinationStatus.Code == 3)
                    {
                        AssignOverallStatus(new Color(1.0f, 0.64f, 0.0f), "High");
                    }
                    else
                    {
                        AssignOverallStatus(Color.yellow, "Moderate");
                    }
                }
                break;

            case 3:
                if(destinationStatus.Code > startingPointStatus.Code)
                {
                    AssignOverallStatus(new Color(1.0f, 0.64f, 0.0f), "High");
                }
                else
                {
                    AssignOverallStatus(Color.yellow, "Moderate");
                }
                break;

            default:
                AssignOverallStatus(destinationStatus.Color, ColorsToStatusDic[destinationStatus.Code]);
                break;
        }
    }

    private void AssignOverallStatus(Color color, string status)
    {
        OverallStatus.text = status;
        OverallStatus.color = color;
    }

    private void AnalyzeStatus()
    {
        startingPointStatus = SetStateStatus(StateConditionsArr[0].Code + (2 * StateConditionsArr[1].Code));
        destinationStatus = SetStateStatus(StateConditionsArr[2].Code + (2 * StateConditionsArr[3].Code));

        //Assign colors
        SStatus.color = startingPointStatus.Color;
        DStatus.color = destinationStatus.Color;
    }

    private Condition SetStateStatus(int value)
    {
        if (value > 13)
            return new Condition(Color.red, 5);

        if (value > 10 && value < 14)
            return new Condition(new Color(1.0f, 0.64f, 0.0f), 4);

        if (value > 7 && value < 11)
            return new Condition(Color.yellow, 3);

        return new Condition(Color.green, 2);
    }

    private void AssignColorsToStats()
    {
        for (byte i = 0; i < StateConditionsArr.Length; i++)
        {
            AnalyzeData(i);
        }

        SetStatsColors();
    }

    private void SetStatsColors()
    {
        SIncidentRate.color = StateConditionsArr[0].Color;
        SCaseFatalityRatio.color = StateConditionsArr[1].Color;
        DIncidentRate.color = StateConditionsArr[2].Color;
        DCaseFatalityRatio.color = StateConditionsArr[3].Color;
    }

    private void AnalyzeData(byte index)
    {
        switch (index)
        {
            case 0: //StartingPoint Incident Rate
                AnalyzeIncidentRate(false);
                break;

            case 1: //StartingPoint Case Fatality Ratio
                AnalyzeCaseFatalityRatio(false);
                break;

            case 2: //Destination Incident Rate
                AnalyzeIncidentRate(true);
                break;

            case 3: //Destination Case Fatality Ratio
                AnalyzeCaseFatalityRatio(true);
                break;
        }
    }

    private void AnalyzeCaseFatalityRatio(bool isDestination)
    {
        if (isDestination)
        {
            double destinationCFRatio = destination.Case_Fatality_Ratio;

            if (destinationCFRatio < 0.5f)
            {
                StateConditionsArr[3] = new Condition(Color.green, 2);
            }
            else
            if (destinationCFRatio >= 0.5f && destinationCFRatio < 1.5f)
            {
                StateConditionsArr[3] = new Condition(Color.yellow, 3);
            }
            else
            if (destinationCFRatio >= 1.5f && destinationCFRatio < 3)
            {
                StateConditionsArr[3] = new Condition(new Color(1.0f, 0.64f, 0.0f), 4); //Orange
            }
            else // >= 3
            {
                StateConditionsArr[3] = new Condition(Color.red, 5);
            }
        }
        else
        {
            double StartingPointCFRatio = startingPoint.Case_Fatality_Ratio;

            if(StartingPointCFRatio < 0.5f)
            {
                StateConditionsArr[1] = new Condition(Color.green, 2);
            }
            else
            if (StartingPointCFRatio >= 0.5f && StartingPointCFRatio < 1.5f)
            {
                StateConditionsArr[1] = new Condition(Color.yellow, 3);
            }
            else
            if (StartingPointCFRatio >= 1.5f && StartingPointCFRatio < 3)
            {
                StateConditionsArr[1] = new Condition(new Color(1.0f, 0.64f, 0.0f), 4); //Orange
            }
            else // >= 3
            {
                StateConditionsArr[1] = new Condition(Color.red, 5);
            }
        }
    }

    private void AnalyzeIncidentRate(bool isDestination)
    {
        if (isDestination) //destination
        {
            double destinationIncidentRate = destination.Incident_Rate;

            if(destinationIncidentRate < 500)
            {
                StateConditionsArr[2] = new Condition(Color.green, 2);
            }
            else
            if(destinationIncidentRate >= 500 && destinationIncidentRate < 1500)
            {

                StateConditionsArr[2] = new Condition(Color.yellow, 3);
            }
            else
            if(destinationIncidentRate >= 1500 && destinationIncidentRate < 3000)
            {
                StateConditionsArr[2] = new Condition(new Color(1.0f, 0.64f, 0.0f), 4); //Orange
            }
            else // >= 3000
            {
                StateConditionsArr[2] = new Condition(Color.red, 5);
            }
        }
        else //starting point
        {
            double StartingPointIncidentRate = startingPoint.Incident_Rate;

            if (StartingPointIncidentRate < 500)
            {
                StateConditionsArr[0] = new Condition(Color.green, 2);
            }
            else
            if (StartingPointIncidentRate >= 500 && StartingPointIncidentRate < 1500)
            {
                StateConditionsArr[0] = new Condition(Color.yellow, 3);
            }
            else
            if (StartingPointIncidentRate >= 1500 && StartingPointIncidentRate < 3000)
            {
                StateConditionsArr[0] = new Condition(new Color(1.0f, 0.64f, 0.0f), 4); //Orange
            }
            else // >= 3000
            {
                StateConditionsArr[0] = new Condition(Color.red, 5);
            }
        }
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

        StateConditionsArr = new Condition[4];
    }

    private class Condition
    {
        public Condition(Color _color, byte _code)
        {
            color = _color;
            code = _code;
        }

        private readonly Color color;
        private readonly byte code;

        public Color Color => color;
        public byte Code => code;
    }
}
