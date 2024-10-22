﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class LocationHandler : MonoBehaviour
{
    [SerializeField] private Location startingPoint;

    private readonly string locationUrl = "https://extreme-ip-lookup.com/json";
    private LocationMaster locationMaster;
    private bool isCurrentLocationCaptured;

    [SerializeField] private TMP_Text currenLocationText;

    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject RetryMenu;

    private void Start()
    {
        SetInitialReferences();
        CheckIfLocationAlreadyCaptured();
    }

    public void CheckIfLocationAlreadyCaptured() //Also called by retry button
    {
        if(!isCurrentLocationCaptured)
            StartCoroutine(GetCurrentLocation());
    }

    private void SetInitialReferences()
    {
        locationMaster = GetComponent<LocationMaster>();
    }

    private IEnumerator GetCurrentLocation()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(locationUrl);

        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            SwapMenus();
        }
        else
        if (webRequest.isDone)
        {
            JsonUtility.FromJsonOverwrite(webRequest.downloadHandler.text, startingPoint);

            string startingPositionKey;

            if (startingPoint.countryCode.Equals("US"))
            {
                startingPositionKey = startingPoint.region + ", " + startingPoint.countryCode;
            }
            else
            {
                startingPositionKey = startingPoint.region + ", " + startingPoint.country;
            }

            currenLocationText.text = startingPoint.city + ", " + startingPoint.country;
            isCurrentLocationCaptured = true;
            locationMaster.CallEventStartingPositionCaptured(startingPositionKey);
        }
    }

    private void SwapMenus()
    {
        MainMenu.SetActive(false);
        RetryMenu.SetActive(true);
    }
}
