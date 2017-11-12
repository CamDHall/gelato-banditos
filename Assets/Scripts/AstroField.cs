using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroField : MonoBehaviour {

    bool populated = false;
    bool turnedOff = false;
    List<GameObject> field = new List<GameObject>();

    public GameObject astroid;
    int size;

    int numAstroids;

    BoxCollider box;
    float camDepth;

    private void Awake()
    {
        box = GetComponent<BoxCollider>();
        numAstroids = Random.Range(40, 50);
        size = AstroSpawner.Instance.col_size;

        camDepth = Camera.main.farClipPlane;
    }

    private void Update()
    {
        if(!populated)
        {
            if(Vector3.Distance(PlayerMovement.player.transform.position, box.ClosestPoint(PlayerMovement.player.transform.position)) < camDepth)
            {
                populated = true;
                Populate();
            }
        } else
        {
            // Turn off if far away
            if(Vector3.Distance(PlayerMovement.player.transform.position, box.ClosestPoint(PlayerMovement.player.transform.position)) > camDepth)
            {
                if (!turnedOff)
                {
                    TurnOff();
                }
            }

            // Turn on if retentering quadrant
            if (Vector3.Distance(PlayerMovement.player.transform.position, box.ClosestPoint(PlayerMovement.player.transform.position)) < camDepth)
            {
                if (turnedOff)
                {
                    turnedOff = false;
                    Populate();
                }
            }
        }

        /* if (!populated)
        {
            if (Vector3.Distance(PlayerMovement.player.transform.position, box.bounds.center) < (box.size.x / 2) + 200)
            {
                populated = true;
                Populate();
                Debug.Log(box.size.x / 2);
            }
        } */
    }
    /*
    private void OnTriggerEnter(Collider coll)
    {
        if(!populated && coll.tag == "Player")
        {
            populated = true;
            Populate();
        } else if(coll.tag == "Player")
        {
            foreach (GameObject astro in field)
            {
                astro.SetActive(true);
            }
        }
    } */

    private void OnTriggerExit(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            foreach(GameObject astro in field)
            {
                astro.SetActive(false);
            }
        }
    }

    void Populate()
    {
        for (int i = 0; i < numAstroids; i++)
        {
            Vector3 Pos = new Vector3((float)Random.Range(-size/2, size/2), (float)Random.Range(-size/2, size/2), (float)Random.Range(-size/2, size/2));
            GameObject temp = Instantiate(astroid);
            temp.transform.parent = transform;
            temp.transform.localPosition = Pos;
            field.Add(temp);
        }
    }

    void TurnOff()
    {
        turnedOff = true;
        Debug.Log("HERE");
        foreach(GameObject astro in field)
        {
            astro.SetActive(false);
        }
    }
}
