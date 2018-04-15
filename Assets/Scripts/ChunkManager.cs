using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class ChunkManager : MonoBehaviour {

    public static ChunkManager Instance;

    public GameObject currentClust;

    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Cluster" && currentClust != other.gameObject)
        {
            Debug.Log(other.gameObject);
            currentClust = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BinaryFormatter bf = new BinaryFormatter();
        //File.WriteAllBytes("/Assets/IO/Test", Utils.ObjectSerializationExtension.SerializeToByteArray((byte[])currentClust));
    }
}
