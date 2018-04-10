using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour {

    public static FieldManager Instance;

    public int checkDist;

    [HideInInspector] public Dictionary<Transform, GameObject> activeClust = new Dictionary<Transform, GameObject>();
    [HideInInspector] public Dictionary<Transform, GameObject> inactiveClusters = new Dictionary<Transform, GameObject>();
    [HideInInspector] public float depth;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        depth = Camera.main.farClipPlane;
    }

    private void Update()
    {
        Check();
    }

    // Turn off clusters that are far away
    void Check()
    {
        List<Transform> inactiveRemoved = new List<Transform>();
        List<Transform> activeRemove = new List<Transform>();
        foreach (Transform pos in inactiveClusters.Keys)
        {
            float distance = Vector3.Distance(transform.position, pos.position);
            if (distance < checkDist)
            {
                inactiveRemoved.Add(pos);
                inactiveClusters[pos].SetActive(true);
            }
        }

        foreach(Transform pos in activeClust.Keys)
        {
            float distance = Vector3.Distance(transform.position, pos.position);
            if(distance >= checkDist)
            {
                activeRemove.Add(pos);
                activeClust[pos].SetActive(false);
            }
        }
    }
}
