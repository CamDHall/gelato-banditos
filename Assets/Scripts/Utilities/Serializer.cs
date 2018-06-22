using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Sirenix.Serialization;

public class Serializer
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

    public static void Save<T>(string filename, T data) where T : class
    {
        DataFormat df = DataFormat.JSON;
        string path = Application.dataPath + "/Data/playerData.txt";

        List<UnityEngine.Object> info = new List<UnityEngine.Object>();
        //List<byte> info = new List<byte>();
        var bytes = SerializationUtility.SerializeValue(data, df, out info);

        File.WriteAllBytes(path, bytes);

        Load(path);
    }

    public static void Load(string name)
    {

    }
}