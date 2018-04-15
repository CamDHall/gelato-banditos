using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChunkManager : MonoBehaviour {

    public static ChunkManager Instance;
    public GameObject galaxyMap;

    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "Cluster") galaxyMap.SetActive(true);
    }
}
