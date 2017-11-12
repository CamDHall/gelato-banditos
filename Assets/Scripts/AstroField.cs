using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroField : MonoBehaviour {

    bool populated = false;
    List<GameObject> field = new List<GameObject>();

    public GameObject astroid;
    int size;

    int numAstroids;

    BoxCollider box;
    float camDepth;


    private void Awake()
    {
        box = GetComponent<BoxCollider>();
        numAstroids = Random.Range(20, 30);
        size = AstroSpawner.Instance.col_size;
        camDepth = Camera.main.farClipPlane;
        Debug.Log(camDepth);
    }

    private void Update()
    {
        if (!populated)
        {
            if (Vector3.Distance(PlayerMovement.player.transform.position, box.ClosestPointOnBounds(PlayerMovement.player.transform.position)) < (camDepth - box.bounds.size.x))
            {
                populated = true;
                Populate();
            }
        }
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
}
