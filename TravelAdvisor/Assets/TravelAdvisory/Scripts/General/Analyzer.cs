using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Analyzer : MonoBehaviour
{
    private LocationMaster locationMasterScript;
    private int[] clusteredData;
    private Dictionary<int, int> priorityDic;

    string[] worldLines;
    string[] usLines;
    private List<string> worldList;

    private void OnEnable()
    {
        SetInitialReferences();
        locationMasterScript.EventDataDownloaded += ProcessCsv;
    }

    private void OnDisable()
    {
        locationMasterScript.EventDataDownloaded -= ProcessCsv;
    }

    private void SetInitialReferences()
    {
        locationMasterScript = GameObject.Find("GameManager").GetComponent<LocationMaster>();
    }

    private void ProcessCsv()
    {
        worldLines = PlayerPrefs.GetString("world").Split('\n');
        usLines = PlayerPrefs.GetString("US").Split('\n');

        worldList = new List<string>();
        List<string> usList;

        for (int i = 1; i < worldLines.Length - 1; i++)
        {
            //651 to 3927 to skip US
            if (i == 33 || (i > 653 && i < 3925)) //Just in Case! *i == 33 is for unknonwn, belgium!*
                continue;

            char temp = worldLines[i][0];
            
            if (!(temp >= '0' && temp <= '9'))
                worldList.Add(worldLines[i]);
        }

        if (!string.IsNullOrEmpty(worldLines[worldLines.Length - 1])) //null check
        {
            worldList.Add(worldLines[worldLines.Length - 1]);
        }

        usList = usLines.Skip(1).ToList();

        if (string.IsNullOrEmpty(usList[usList.Count - 1])) //null check
        {
            usList.RemoveAt(usList.Count - 1);
        }

        worldList.AddRange(usList);

        AnalyzeData();
    }

    private void AnalyzeData()
    {
        double[][] rawData = new double[worldList.Count][];
        double ir, cfr;

        int usStartIndex = worldList.Count - 58; //58 US States

        for (int i = 0; i < worldList.Count; i++)
        {
            if (i < usStartIndex) //704
            {
                string line = worldList[i].Replace("\"", string.Empty);
                string[] column = line.Split(',');

                if (column[3].Equals("Korea"))
                {
                    ir = column[14].Equals("") ? 0f : Convert.ToDouble(column[14]);
                    cfr = column[15].Equals("") ? 0f : Convert.ToDouble(column[15]);
                    rawData[i] = new double[] { ir, cfr * 1000 };
                    continue;
                }

                if (column[2].Equals("Bonaire") || column[2].Equals("Saint Helena"))
                {
                    ir = column[15].Equals("") ? 0f : Convert.ToDouble(column[15]);
                    cfr = column[16].Equals("") ? 0f : Convert.ToDouble(column[16]);
                    rawData[i] = new double[] { ir, cfr * 1000 };
                    continue;
                }

                if (column[2].Equals("")) //if state/region is not specified in the dataset
                {
                    ir = column[12].Equals("") ? 0f : Convert.ToDouble(column[12]);
                    cfr = string.IsNullOrWhiteSpace(column[13]) ? 0f : Convert.ToDouble(column[13]);
                }
                else
                {
                    ir = column[13].Equals("") ? 0f : Convert.ToDouble(column[13]);
                    cfr = string.IsNullOrWhiteSpace(column[14]) ? 0f : Convert.ToDouble(column[14]);
                }
            }
            else //US
            {
                string[] column = worldList[i].Split(',');
                ir = column[10].Equals("") ? 0f : Convert.ToDouble(column[10]);
                cfr = column[13].Equals("") ? 0f : Convert.ToDouble(column[13]);
            }

            rawData[i] = new double[] { ir, cfr * 1000 };
        }

        clusteredData = ClusteringKMeans.KMeansDemo.Cluster(rawData, 4); // this is it

        Dictionary<int, ClusterStates> clustersDic = new Dictionary<int, ClusterStates>()
        {
            {0, new ClusterStates() },
            {1, new ClusterStates() },
            {2, new ClusterStates() },
            {3, new ClusterStates() }
        };

        for (int i = 0; i < clusteredData.Length; i++)
        {
            clustersDic[clusteredData[i]].value += rawData[i][0] + rawData[i][1];

            clustersDic[clusteredData[i]].indexCount++;
        }

        for (int i = 0; i < clustersDic.Count; i++)
        {
            clustersDic[i].value /= clustersDic[i].indexCount;
        }

        //Queue<int> priorityQueue = new Queue<int>(new[] { 2, 1, 3, 0 });
        Queue<int> priorityQueue = new Queue<int>(new[] { 3, 2, 1, 0 });

        priorityDic = new Dictionary<int, int>();

        while (priorityQueue.Count > 0)
        {
            int maxKey = FindMax(clustersDic);
            clustersDic.Remove(maxKey);
            priorityDic.Add(maxKey, priorityQueue.Dequeue());
        }

        SaveData();
    }

    private void SaveData()
    {
        SaveSystem.SaveData(worldList, clusteredData, priorityDic);
        locationMasterScript.CallEventDataAnalyzed();
    }

    private int FindMax(Dictionary<int, ClusterStates> clustersDic)
    {
        int maxIndex = 0;
        double maxValue = 0;
        
        foreach (var item in clustersDic)
        {
            if(item.Value.value > maxValue)
            {
                maxValue = item.Value.value;
                maxIndex = item.Key;
            }
        }

        return maxIndex;
    }

    private class ClusterStates
    {
        public int indexCount;
        public double value;
    }
}