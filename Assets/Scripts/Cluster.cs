using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cluster : Field {

    public float xWidth, yWidth, zDepth;

    private void Awake()
    {
        numAstroids = Random.Range(numLow, numHigh);
        size = clustSize / 2;

        astroSize = bigAstro.GetComponent<SphereCollider>().radius;
    }

    private void Start()
    {
        Pos = transform.position;
    }

    public override void Populate()
    {
        base.Populate();

        ///
        /// The cluster is random, but I'm using xWidth, yWidth, and zDepth to modify the general shape so that it is wide and short
        ///
        for (int i = 0; i < numAstroids; i++)
        {
            Vector3 Pos = new Vector3((float)Random.Range(-size / xWidth, size / xWidth), (float)Random.Range(-size / yWidth, size / yWidth), 
                (float)Random.Range(-size / zDepth, size / zDepth));
            GameObject temp = Instantiate(bigAstro);
            temp.transform.parent = transform;
            temp.transform.localPosition = Pos;
            field.Add(temp);

            //AstroSpawner.Instance.astroids.Add(temp, Random.Range(2, 5));
            AstroSpawner.Instance.astroids.Add(temp, 1);

            // For every astero, randomly choice to spawn a bandito
            if (Random.Range(0, 100) < spawnChance)
            {
                BanditoSpawner.Instance.SpawnEnemies(temp, gameObject);
            }
        }
    }
}
