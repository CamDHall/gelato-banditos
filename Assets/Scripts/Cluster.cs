using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cluster : Field {

    public float xWidth, yWidth, zDepth;

    private void Awake()
    {
        numAstroids = Random.Range(numLow, numHigh);
        size = clustSize * 0.8f;
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
            GameObject temp = Instantiate(bigAstro);
            temp.transform.SetParent( transform);

            if (transform.position.x == 5 && transform.position.y == 5 && transform.position.z == 5)
            {
                temp.transform.localPosition = AsteroidUtil.CenterPlace(size, xWidth, yWidth, zDepth);
            } else
            {
                temp.transform.localPosition = AsteroidUtil.Placement(size, xWidth, yWidth, zDepth);
            }

            temp.transform.localRotation = AsteroidUtil.Rotation();
            temp.transform.localScale = AsteroidUtil.Scale();

            AsteroidUtil.DetermineCollider(temp);

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
