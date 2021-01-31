using UnityEngine;

public class OpenURL : MonoBehaviour
{
    public void OpenUrl(string url) //Called by hyperlink
    {
        if (!string.IsNullOrWhiteSpace(url))
        {
            Application.OpenURL(url);
        }
    }
}
