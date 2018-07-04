using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Sirenix.Serialization;
using System.Linq;

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

    public class MyData
    {
        public string str = new string(Enumerable.Range(0, 20).Select(i => (char)UnityEngine.Random.Range(50, 150)).ToArray());
        public List<float> numbers = new List<float>(Enumerable.Range(0, 10).Select(i => UnityEngine.Random.Range(0f, 100f)));
        public GameObject unityObjectReference = UnityEngine.Object.FindObjectOfType<UnityEngine.GameObject>();
        public MyData reference;
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