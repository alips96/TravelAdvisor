using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class LocationHandler : MonoBehaviour
{
    [SerializeField] private Location startingPoint;
    private readonly string locationUrl = "https://extreme-ip-lookup.com/json";
    private LocationMaster locationMaster;
    private bool isCurrentLocationCaptured;

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
            Debug.LogError(webRequest.error);
            SwapMenus();
        }
        else
        if (webRequest.isDone)
        {
            JsonLocationData jsonData = JsonUtility.FromJson<JsonLocationData>(webRequest.downloadHandler.text);

            startingPoint.businessName = jsonData.businessName;
            startingPoint.city = jsonData.city;
            startingPoint.continent = jsonData.continent;
            startingPoint.country = jsonData.country;
            startingPoint.countryCode = jsonData.countryCode;
            startingPoint.isp = jsonData.isp;
            startingPoint.lat = jsonData.lat;
            startingPoint.lon = jsonData.lon;
            startingPoint.org = jsonData.org;
            startingPoint.query = jsonData.query;
            startingPoint.state = jsonData.region;
            startingPoint.status = jsonData.status;

            string startingPositionKey = startingPoint.state.ToLower() + ", " + startingPoint.country.ToLower();
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

public class JsonLocationData
{

    public string businessName;
    public string businessWebsite;
    public string city;
    public string continent;
    public string country;
    public string countryCode;
    public string ipName;
    public string ipType;
    public string isp;
    public string lat;
    public string lon;
    public string org;
    public string query;
    public string region;
    public string status;

}
