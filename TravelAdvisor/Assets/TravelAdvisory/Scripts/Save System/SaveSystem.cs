using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveSystem
{
    public static void SaveData(List<string> _worldList, int[] _clusteredData, Dictionary<int, int> _priorityDic)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "covidData.txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        CovidData data = new CovidData(_worldList, _clusteredData, _priorityDic);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static CovidData LoadData()
    {
        string path = Application.persistentDataPath + "covidData.txt";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            CovidData data = formatter.Deserialize(stream) as CovidData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save not found in " + path);
            return null;
        }
    }
}
