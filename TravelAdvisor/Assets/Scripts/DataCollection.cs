using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class DataCollection : MonoBehaviour
{
    string yesterday;

    void Start()
    {
        yesterday = GetYesterday();
        bool shouldDownloadCorpus = CheckIfDownloadNecessary(yesterday);

        if (shouldDownloadCorpus)
        {
            PlayerPrefs.DeleteAll();
            StartCoroutine(DownloadWorldCorpus());
            StartCoroutine(DownloadUSCorpus());
            PlayerPrefs.SetInt(yesterday, 1);
        }
    }

    private IEnumerator DownloadUSCorpus()
    {
        string url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports_us/" + yesterday + ".csv";

        UnityWebRequest webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
        string path = Path.Combine(Application.dataPath + "/Editor/Data", "USCovidData.csv");
        webRequest.downloadHandler = new DownloadHandlerFile(path);

        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
            Debug.LogError(webRequest.error);
    }

    private IEnumerator DownloadWorldCorpus()
    {
        string url = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/" + yesterday + ".csv";

        UnityWebRequest webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
        string path = Path.Combine(Application.dataPath + "/Editor/Data", "covidData.csv");
        webRequest.downloadHandler = new DownloadHandlerFile(path);

        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
            Debug.LogError(webRequest.error);
    }

    private bool CheckIfDownloadNecessary(string yesterday)
    {
        return PlayerPrefs.GetInt(yesterday) != 1;
    }

    private string GetYesterday()
    {
        DateTime yesterday = DateTime.Today.AddDays(-1); //because we need to download the data of the previous day.

        int day = yesterday.Day;
        int month = yesterday.Month;
        string dayCorrectFormat = day < 10 ? "0" + day.ToString() : day.ToString();
        string monthCorrectFormat = month < 10 ? "0" + month.ToString() : month.ToString();

        return monthCorrectFormat + "-" + dayCorrectFormat + "-" + yesterday.Year.ToString();
    }
}
