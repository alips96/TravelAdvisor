using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DataCollection : MonoBehaviour
{
    [HideInInspector] public string targetDay;

    private bool isDataCaptured;
    private bool shouldDownloadData;

    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject RetryMenu;

    private LocationMaster locationMasterScript;

    byte fileCounter = 0;

    void Start()
    {
        SetInitialReferences();
        //PlayerPrefs.DeleteAll();

        if (DateTime.Now.Hour > 7) //Files get uploaded after 7 a.m :)
        {
            targetDay = GetTargetDay(-1); // yesterday
        }
        else
        {
            targetDay = GetTargetDay(-2); // To handle late upload.
        }

        shouldDownloadData = CheckIfDownloadNecessary(targetDay);

        if (shouldDownloadData)
        {
            PlayerPrefs.DeleteAll();
            InitiateDownload();
            PlayerPrefs.SetInt(targetDay, 1);
        }
        else
        {
            locationMasterScript.CallEventDataAnalyzed();
        }
    }

    private void SetInitialReferences()
    {
        locationMasterScript = GetComponent<LocationMaster>();
    }

    public void InitiateDownload() //Also called by retry button
    {
        if (!isDataCaptured)
        {
            //World except US
            string worldUrl = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/" + targetDay + ".csv";

            //US
            string USUrl = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports_us/" + targetDay + ".csv";

            isDataCaptured = true;
            _ = StartCoroutine(DownloadRawData(worldUrl, "world"));
            _ = StartCoroutine(DownloadRawData(USUrl, "US"));
        }
    }

    private IEnumerator DownloadRawData(string url, string key)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(url);

        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            isDataCaptured = false;
            SwapMenus();
        }
        else if (webRequest.isDone)
        {
            PlayerPrefs.SetString(key, webRequest.downloadHandler.text);
            fileCounter++;

            if (fileCounter == 2)
            {
                locationMasterScript.CallEventDataDownloaded(); //both files downloaded successfully
            }
        }
    }

    private void SwapMenus()
    {
        MainMenu.SetActive(false);
        RetryMenu.SetActive(true);
    }

    private bool CheckIfDownloadNecessary(string yesterday)
    {
        return PlayerPrefs.GetInt(yesterday) != 1;
    }

    private string GetTargetDay(int daysAdded)
    {
        DateTime yesterday = DateTime.Today.AddDays(daysAdded); //we need to download the data of the previous days.

        int day = yesterday.Day;
        int month = yesterday.Month;
        string dayCorrectFormat = day < 10 ? "0" + day.ToString() : day.ToString();
        string monthCorrectFormat = month < 10 ? "0" + month.ToString() : month.ToString();

        return monthCorrectFormat + "-" + dayCorrectFormat + "-" + yesterday.Year.ToString();
    }
}
