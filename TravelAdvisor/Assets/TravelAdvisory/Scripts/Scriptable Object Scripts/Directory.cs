using UnityEngine;

[CreateAssetMenu(fileName = "New Directory", menuName = "Assets/ScriptibleObjects/New Directory")]
public class Directory : ScriptableObject
{
    public string worldDataUrl;
    public string usDataUrl;
    public string worldSavePath;
    public string usSavePath;
    public string currentLocationUrl;
}