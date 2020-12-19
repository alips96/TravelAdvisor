using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using System.IO;
using UnityEditor;
using System.Linq;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class SaveData : MonoBehaviour
{
    private readonly string dataCsvPath = "/TravelAdvisory/Data/covidData.csv";
    private readonly string usDataCsvPath = "/TravelAdvisory/Data/USCovidData.csv";

    [SerializeField] private GameObject loadingMenu;
    [SerializeField] private GameObject resultMenu;

    LinkedList<Action> funtionsToRunInMainThread = new LinkedList<Action>();

    string[] worldAllLines;
    string[] usAllLines;

    int i = 0;

    private void Update()
    {
        while (funtionsToRunInMainThread.Count > 0)
        {
            Action someFuntion = funtionsToRunInMainThread.First.Value;
            funtionsToRunInMainThread.RemoveFirst();
            i++;
            someFuntion();
        }

        //if(funtionsToRunInMainThread.Count > 0)
        //{
        //    Action someFuntion = funtionsToRunInMainThread.First.Value;
        //    funtionsToRunInMainThread.RemoveFirst();
        //    i++;
        //    Debug.Log(funtionsToRunInMainThread.Count);
        //    someFuntion();
        //}

        if (i > 757)
        {
            Debug.Log("finish");
            AssetDatabase.SaveAssets();
            loadingMenu.SetActive(false);
            resultMenu.SetActive(true);

            enabled = false;
        }
    }

    private void StartThreadedFuntion(Action someFuntionWithNoParams)
    {
        Thread t = new Thread(new ThreadStart(someFuntionWithNoParams));
        t.Start();
    }

    private void SlowFuntion(int startIndex, int lastIndex)
    {
        LinkedList<Foo> fooLinkedList = new LinkedList<Foo>();

        for (int i = startIndex; i <= lastIndex; i++)
        {
            string line = worldAllLines[i].Replace("\"", string.Empty);
            line = line.Replace("*", string.Empty);
            string[] column = line.Split(',');

            Foo myFoo = new Foo();

            if (column[3] == "Korea" || column[2] == "Bonaire")
            {
                continue;
            }
            if (column[2].CompareTo("") == 0) //if state/region is specified in the dataset
            {
                myFoo.state = string.Empty; //this means the stats are provided only for the  country.
                myFoo.country = column[3];
                myFoo.confirmed = Convert.ToInt32(column[7]);
                myFoo.deaths = Convert.ToInt32(column[8]);
                myFoo.recovered = Convert.ToInt32(column[9]);
                myFoo.active = column[10].CompareTo("") == 0 ? 0 : Convert.ToInt32(column[10]);
                myFoo.combinedKey = column[11].ToLower();
                myFoo.incidentrate = column[12].CompareTo("") == 0 ? 0f : Convert.ToDouble(column[12]);
                myFoo.caseFatality = column[13].CompareTo("") == 0 ? 0f : Convert.ToDouble(column[13]);
                fooLinkedList.AddFirst(myFoo);
            }
            else
            {
                myFoo.state = column[2];
                myFoo.country = column[3];
                myFoo.confirmed = Convert.ToInt32(column[7]);
                myFoo.deaths = Convert.ToInt32(column[8]);
                myFoo.recovered = Convert.ToInt32(column[9]);
                myFoo.active = column[10].CompareTo("") == 0 ? 0 : Convert.ToInt32(column[10]);
                myFoo.combinedKey = column[11].ToLower() + "," + column[12].ToLower();
                myFoo.incidentrate = column[13].CompareTo("") == 0 ? 0f : Convert.ToDouble(column[13]);
                myFoo.caseFatality = column[14].CompareTo("") == 0 ? 0f : Convert.ToDouble(column[14]);
                fooLinkedList.AddFirst(myFoo);
            }
        }

        Action aFuntion = () =>
        {
            foreach (Foo item in fooLinkedList)
            {
                Region region = ScriptableObject.CreateInstance<Region>();

                region.Province_State = item.state;
                region.Country_Region = item.country;
                region.Confirmed = item.confirmed;
                region.Deaths = item.deaths;
                region.Recovered = item.recovered;
                region.Active = item.active;
                region.Combined_Key = item.combinedKey;
                region.Incident_Rate = item.incidentrate;
                region.Case_Fatality_Ratio = item.caseFatality;

                AssetDatabase.CreateAsset(region, $"Assets/TravelAdvisory/ScriptableObjects/Regions/Resources/{region.Combined_Key}.asset");
            }
        };

        QueueMainThreadFuntion(aFuntion);
    }

    private void SlowFuntionUS()
    {
        LinkedList<Foo> fooLinkedList = new LinkedList<Foo>();

        foreach (string line in usAllLines.Skip(1))
        {
            string[] column = line.Split(',');
            Foo myFoo = new Foo();

            myFoo.state = column[0];
            //region.Country_Region = "US";
            myFoo.confirmed = Convert.ToInt32(column[5]);
            myFoo.deaths = Convert.ToInt32(column[6]);
            myFoo.recovered = column[7].CompareTo("") == 0 ? 0 : (int)float.Parse(column[7]);
            myFoo.active = (int)float.Parse(column[8]);
            myFoo.incidentrate = column[10].CompareTo("") == 0 ? 0f : Convert.ToDouble(column[10]);
            myFoo.caseFatality = column[13].CompareTo("") == 0 ? 0f : Convert.ToDouble(column[13]);
            myFoo.combinedKey = column[0].ToLower() + ", us";

            fooLinkedList.AddFirst(myFoo);
        }

        Action aFuntion = () =>
        {
            foreach (Foo item in fooLinkedList)
            {
                Region region = ScriptableObject.CreateInstance<Region>();

                region.Province_State = item.state;
                region.Country_Region = "US";
                region.Confirmed = item.confirmed;
                region.Deaths = item.deaths;
                region.Recovered = item.recovered;
                region.Active = item.active;
                region.Combined_Key = item.combinedKey;
                region.Incident_Rate = item.incidentrate;
                region.Case_Fatality_Ratio = item.caseFatality;

                AssetDatabase.CreateAsset(region, $"Assets/TravelAdvisory/ScriptableObjects/Regions/Resources/{region.Combined_Key}.asset");
            }
        };

        QueueMainThreadFuntion(aFuntion);
    }

    public void ProcessData() //should be called by an event
    {
        //Debug.Log("start Processing data");
        loadingMenu.SetActive(true);
        worldAllLines = File.ReadAllLines(Application.dataPath + dataCsvPath);
        usAllLines = File.ReadAllLines(Application.dataPath + usDataCsvPath);
        StartThreadedFuntion(() => { SlowFuntion(1, 300); });
        StartThreadedFuntion(() => { SlowFuntion(301, 649); });
        StartThreadedFuntion(() => { SlowFuntion(3924, worldAllLines.Length - 1); });
        StartThreadedFuntion(() => { SlowFuntionUS(); });
    }

    void QueueMainThreadFuntion(Action someFuntion)
    {
        funtionsToRunInMainThread.AddFirst(someFuntion);
    }

    public class Foo
    {
        public string state;
        public string country;
        public int confirmed;
        public int deaths;
        public int recovered;
        public int active;
        public double incidentrate;
        public double caseFatality;
        public string combinedKey;
    }
}
