using System;
using System.Collections;
using System.IO;
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
            DownloadRawData();
            PlayerPrefs.SetInt(targetDay, 1);
        }
        else
        {
            locationMasterScript.CallEventDataDownloaded();
        }
    }

    private void SetInitialReferences()
    {
        locationMasterScript = GetComponent<LocationMaster>();
    }

    public void DownloadRawData() //Also called by retry button
    {
        if (!isDataCaptured)
            DownloadWorldCorpus();
    }

    private void DownloadWorldCorpus()
    {
        foreach (string item in Directory.GetFiles(Application.dataPath + "/TravelAdvisory/Data"))
        {
            File.Delete(item);
        }

        //World except US
        string worldUrl = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/" + targetDay + ".csv";
        string worldPath = Application.dataPath + "/TravelAdvisory/Data/covidData.csv";

        //US
        string USUrl = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports_us/" + targetDay + ".csv";
        string UsPath = Application.dataPath + "/TravelAdvisory/Data/UScovidData.csv";

        isDataCaptured = true;
        StartCoroutine(DownloadData(worldUrl, worldPath, "world"));
        StartCoroutine(DownloadData(USUrl, UsPath, "US"));
    }

    private IEnumerator DownloadData(string url, string path, string key)
    {
        Debug.Log(path);
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
