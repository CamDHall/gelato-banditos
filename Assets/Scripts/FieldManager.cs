using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour {

    public static FieldManager Instance;
    public int checkDist;
    public float requiredDist;

    [HideInInspector] public Dictionary<Vector3, GameObject> activeClust = new Dictionary<Vector3, GameObject>();
    [HideInInspector] public Dictionary<Vector3, GameObject> inactiveClusters = new Dictionary<Vector3, GameObject>();
    [HideInInspector] public float depth;

    List<Vector3> neighbors = new List<Vector3>();
    Vector3 lastPos;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        lastPos = transform.position;
        GetNeighbors();
        Check();
    }

    private void Update()
    {
        if (Vector3.Distance(lastPos, transform.position) > requiredDist)
        {
            GetNeighbors();
            Check();
        }
    }

    // Turn off clusters that are far away
    void Check()
    {
        foreach(Vector3 pos in neighbors)
        {
            float distance = Vector3.Distance(transform.position, pos);

   
            if(distance < checkDist && inactiveClusters.ContainsKey(pos))
            {
                GameObject clust = inactiveClusters[pos];
                activeClust.Add(pos, clust);
                clust.SetActive(true);
                inactiveClusters.Remove(pos);
            }
        }

        List<Vector3> activeKeys = new List<Vector3>(activeClust.Keys);

        foreach (Vector3 pos in activeKeys)
        {
            float distance = Vector3.Distance(transform.position, pos);
            
            if(distance >= checkDist)
            {
                GameObject clust = activeClust[pos];
                activeClust.Remove(pos);
                inactiveClusters.Add(pos, clust);
                clust.SetActive(false);
            }
        }
    }

    void GetNeighbors()
    {
        List<Vector3> temp = new List<Vector3>();

        foreach(Vector3 pos in inactiveClusters.Keys)
        {
            if(Vector3.Distance(pos, transform.position) <= (checkDist * 2))
            {
                temp.Add(pos);
            }
        }

        foreach (Vector3 pos in activeClust.Keys)
        {
            if (Vector3.Distance(pos, transform.position) <= (checkDist * 2))
            {
                temp.Add(pos);
            }
        }

        neighbors = temp;
    }
}
