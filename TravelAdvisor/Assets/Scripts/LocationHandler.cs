using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LocationHandler : MonoBehaviour
{
    [SerializeField] private Location startingPoint;
    private readonly string locationUrl = "https://extreme-ip-lookup.com/json";

    private void Start()
    {
        StartCoroutine(GetCurrentLocation());
    }

    private IEnumerator GetCurrentLocation()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(locationUrl);

        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
            Debug.LogError(webRequest.error);
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
        }
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
