using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Sirenix.Serialization;
public class DataManager
{
    public static T Load<T>(string filename) where T : class
    {
        if (File.Exists(filename))
        {
            try
            {
                using (Stream stream = File.OpenRead(filename))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return formatter.Deserialize(stream) as T;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        return default(T);
    }

    // Somewhere, a method to serialize data to json might look something like this
    public static void Save<T>(T data) where T : class
    {
        string path = Application.dataPath + "/Data/playerInventory.dat";

        List<UnityEngine.Object> unityObjectReferences = new List<UnityEngine.Object>();

        DataFormat dataFormat = DataFormat.Binary;

        var bytes = SerializationUtility.SerializeValue(data, dataFormat, out unityObjectReferences);
        File.WriteAllBytes(path, bytes);
    }

    public static PlayerData LoadCharacterData()
    {
        string path = Application.dataPath + "/Data/playerInventory.dat";
        DataFormat df = DataFormat.Binary;
        List<UnityEngine.Object> objs = new List<UnityEngine.Object>();

        var bytes = File.ReadAllBytes(path);
        PlayerData data = SerializationUtility.DeserializeValue<PlayerData>(bytes, df, objs);

        return data;
    }
}