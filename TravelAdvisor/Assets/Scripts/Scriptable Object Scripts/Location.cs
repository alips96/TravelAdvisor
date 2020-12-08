using UnityEngine;

[CreateAssetMenu(fileName = "New Location", menuName = "Assets/ScriptibleObjects/New Location")]
public class Location : ScriptableObject
{
    public string businessName;
    public string city;
    public string continent;
    public string country;
    public string countryCode;
    public string isp;
    public string lat;
    public string lon;
    public string org;
    public string query;
    public string state;
    public string status;
}
