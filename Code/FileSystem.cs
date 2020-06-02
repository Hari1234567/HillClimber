using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class FileSystem
{
    public static void saveData()
    {

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.hill";
        FileStream stream = new FileStream(path, FileMode.Create);
        SaveData saveData = new SaveData();
        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static SaveData loadData()
    {
        string path = Application.persistentDataPath + "/player.hill";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData saveData = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return saveData;
        }
        else
        {
            return null;
        }
    }
}
