using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroField : Field {

    private void Awake()
    {
        numAstroids = Random.Range(numLow, numHigh);
        size = AstroSpawner.Instance.col_size / 2;

        camDepth = Camera.main.farClipPlane + 100;
        box = GetComponent<BoxCollider>();
        astroSize = astroid.GetComponent<BoxCollider>().size.x;
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
