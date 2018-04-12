using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroField : Field {

    private void Awake()
    {
        numAstroids = Random.Range(numLow, numHigh);
        size = AstroSpawner.Instance.col_size / 2;

        astroSize = astroid.GetComponent<BoxCollider>().size.x;
    }

    private void Start()
    {
        Pos = transform.position;
    }

    public override void Populate()
    {
        base.Populate();
        for (int i = 0; i < numAstroids; i++)
        {
            Vector3 Pos = new Vector3((float)Random.Range(-size + astroSize, size - astroSize), 
                (float)Random.Range(-size + astroSize, size - astroSize), 
                (float)Random.Range(-size + astroSize, size - astroSize));
            GameObject temp = Instantiate(astroid);
            temp.transform.parent = transform;
            temp.transform.localPosition = Pos;
            AstroSpawner.Instance.astroids.Add(temp, Random.Range(2, 5));

            field.Add(temp);
        }
    }
}
