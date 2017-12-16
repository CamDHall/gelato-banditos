using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour {

    public static FieldManager Instance;

    public int rayOffset;
    public float checkWaitLength;
    float turnOffTimer, turnOnTimer;

    [HideInInspector] public List<GameObject> activeClust = new List<GameObject>();
    [HideInInspector] public Dictionary<Transform, GameObject> clusters = new Dictionary<Transform, GameObject>();
    [HideInInspector] public float depth;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        depth = 2000;
        turnOnTimer = Time.timeSinceLevelLoad + checkWaitLength;
    }

    private void Update()
    {
        // Check turn on and turn off every 3 seconds, offset the turn on and turn off check
        if (turnOnTimer < Time.timeSinceLevelLoad)
        {
            RaycastHit hit;

            // Set ray checks to be wider than ship
            Vector3 right = transform.position + (transform.right * rayOffset);
            Vector3 left = transform.position - (transform.right * rayOffset);

            // If the raycast hits a cluster's collider, either populate or turn it on
            if (Physics.Raycast(transform.position, transform.forward, out hit, depth, LayerMask.GetMask("Cluster"), QueryTriggerInteraction.Collide) ||
                Physics.Raycast(left, transform.forward, out hit, depth, LayerMask.GetMask("Cluster"), QueryTriggerInteraction.Collide) ||
                Physics.Raycast(right, transform.forward, out hit, depth, LayerMask.GetMask("Cluster"), QueryTriggerInteraction.Collide))
            {
                Cluster field = hit.transform.GetComponent<Cluster>();

                if (!field.populated)
                {
                    field.Populate();
                }
                else if (field.turnedOff)
                {
                    field.TurnOn();
                }

                activeClust.Add(hit.transform.gameObject);
            }

            turnOnTimer = Time.timeSinceLevelLoad + checkWaitLength;
        }

        if (Time.timeSinceLevelLoad > turnOffTimer)
        {
            Check();
            turnOffTimer = Time.timeSinceLevelLoad + checkWaitLength;
        }
    }

    // Turn off clusters that are far away
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
