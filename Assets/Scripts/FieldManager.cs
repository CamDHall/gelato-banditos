using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour {

    public static FieldManager Instance;

    public int rayOffset;
    public float checkWaitLength;
    float checkTimer;

    [HideInInspector] public List<GameObject> activeClust = new List<GameObject>();
    [HideInInspector] public Dictionary<Transform, GameObject> clusters = new Dictionary<Transform, GameObject>();
    [HideInInspector] public float camDepth;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        camDepth = Camera.main.farClipPlane + 100;
    }

    private void Update()
    {
        RaycastHit hit;

        Vector3 right = transform.position + (transform.right * rayOffset);
        Vector3 left = transform.position - (transform.right * rayOffset);
        Vector3 up = transform.position + (transform.up * rayOffset);
        Vector3 down = transform.position - (transform.up * rayOffset);

        Debug.DrawRay(right, transform.forward * camDepth, Color.white);

        if(Physics.Raycast(transform.position, transform.forward, out hit, camDepth, LayerMask.GetMask("Cluster"), QueryTriggerInteraction.Collide) ||
            Physics.Raycast(left, transform.forward, out hit, camDepth, LayerMask.GetMask("Cluster"), QueryTriggerInteraction.Collide) ||
            Physics.Raycast(right, transform.forward, out hit, camDepth, LayerMask.GetMask("Cluster"), QueryTriggerInteraction.Collide) ||
            Physics.Raycast(up, transform.forward, out hit, camDepth, LayerMask.GetMask("Cluster"), QueryTriggerInteraction.Collide) ||
            Physics.Raycast(down, transform.forward, out hit, camDepth, LayerMask.GetMask("Cluster"), QueryTriggerInteraction.Collide))
        {
            Cluster field = hit.transform.GetComponent<Cluster>();

            if(!field.populated)
            {
                field.Populate();
            } else if(field.turnedOff)
            {
                field.TurnOn();
            }

            activeClust.Add(hit.transform.gameObject);
        }

        if(Time.timeSinceLevelLoad > checkTimer)
        {
            Check();
            checkTimer = Time.timeSinceLevelLoad + checkWaitLength;
        }
    }

    void Check()
    {
        List<GameObject> newActive = new List<GameObject>();
        foreach(GameObject chunk in activeClust)
        {
            if(Vector3.Distance(chunk.transform.position, transform.position) > 10000)
            {
                chunk.GetComponent<Cluster>().TurnOff();
            } else
            {
                newActive.Add(chunk);
            }
        }

        activeClust = newActive;
    }
}
