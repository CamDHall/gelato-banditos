using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroField : MonoBehaviour {

    bool populated = false;
    List<GameObject> field;

    int size;

    int numAstroids;

    private void Awake()
    {
        numAstroids = Random.Range(20, 40);
        size = AstroSpawner.Instance.col_size;
    }

    public GameObject astroid;

    private void OnTriggerStay(Collider coll)
    {
        if(!populated && coll.tag == "Player")
        {
            populated = true;
            Populate();
        }
    }

    void Populate()
    {
        for (int i = 0; i < numAstroids; i++)
        {
            Vector3 Pos = new Vector3((float)Random.Range(-size, size), (float)Random.Range(-size, size), (float)Random.Range(-size, size));
            GameObject temp = Instantiate(astroid);
            temp.transform.parent = transform;
            temp.transform.position = Pos;
            Debug.Log(temp.transform.parent);
        }
    }
}
