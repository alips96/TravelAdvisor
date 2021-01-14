using System.Collections.Generic;

[System.Serializable]
public class CovidData
{
    public List<string> worldList;
    public int[] clusteredData;
    public Dictionary<int, int> priorityDic;

    public CovidData(List<string> wList, int[] clusData, Dictionary<int, int> prioDic)
    {
        worldList = wList;
        clusteredData = clusData;
        priorityDic = prioDic;
    }

    public int GetCorrespondingSituationIndex(int i)
    {
        return priorityDic[clusteredData[i]];
    }
}
