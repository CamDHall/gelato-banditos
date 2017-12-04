using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cluster : Field {

    private void Awake()
    {
        numAstroids = Random.Range(numLow, numHigh);
        size = AstroSpawner.Instance.col_size / 2;

        camDepth = Camera.main.farClipPlane + 100;
        box = GetComponent<BoxCollider>();
        astroSize = bigAstro.GetComponent<BoxCollider>().size.x;
    }

    private void Start()
    {
        Pos = transform.position;
    }

    private void Update()
    {
        playerDist = Vector3.Distance(PlayerMovement.player.transform.position, Pos);

        // Turn off if far away
        if (playerDist > camDepth)
        {
            if (!turnedOff)
            {
                TurnOff();
            }
        }

        if (playerDist < camDepth)
        {
            if (!populated)
            {
                Populate();
            }
            else
            {
                if (turnedOff)
                {
                    TurnOn();
                }
            }
        }
    }

    public override void Populate()
    {
        base.Populate();
        for (int i = 0; i < numAstroids; i++)
        {
            Vector3 Pos = new Vector3((float)Random.Range(-size / 1.5f, size / 1.5f), (float)Random.Range(-size / 3, size / 3), 
                (float)Random.Range(-size / 2, size / 2));
            GameObject temp = Instantiate(bigAstro);
            temp.transform.parent = transform;
            temp.transform.localPosition = Pos;
            field.Add(temp);
            AstroSpawner.Instance.astroids.Add(temp, Random.Range(2, 5));
            //AstroSpawner.Instance.astroids.Add(temp, 1);

            if (Random.Range(0, 100) < spawnChance)
            {
                BanditoSpawner.Instance.SpawnEnemies(temp, gameObject);
            }
        }
    }
}
