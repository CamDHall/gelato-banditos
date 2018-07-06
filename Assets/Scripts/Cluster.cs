using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cluster : AsteroidField {

    public float clustRadius;
    public List<GameObject> astro_choices;

    private void Awake()
    {
        numAstroids = Random.Range(numLow, numHigh);
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
            int choice = Random.Range(0, astro_choices.Count);

            GameObject temp = Instantiate(astro_choices[choice]);
            temp.transform.SetParent( transform);

            temp.transform.localPosition = PlayerMovement.player.transform.position + (Random.insideUnitSphere * clustRadius);

            temp.transform.localRotation = AsteroidUtil.Rotation();
            temp.transform.localScale = AsteroidUtil.Scale();

            float largestScale;
            if (temp.transform.localScale.x >= temp.transform.localScale.y && 
                temp.transform.localScale.x >= temp.transform.localScale.z)
                largestScale = transform.localScale.x;
            else if (transform.localScale.y > transform.localScale.z)
                largestScale = transform.localScale.y;
            else
                largestScale = transform.localScale.z;

            Collider[] colls = Physics.OverlapSphere(temp.transform.position, largestScale);

            if(colls.Length > 0)
            {
                Destroy(temp);
                continue;
            }

            AsteroidUtil.DetermineCollider(temp);

            field.Add(temp);

            //AstroSpawner.Instance.astroids.Add(temp, Random.Range(2, 5));
            //AstroSpawner.Instance.astroids.Add(temp, 1);

            // For every astero, randomly choice to spawn a bandito
            if (Random.Range(0, 100) < spawnChance)
            {
                BanditoSpawner.Instance.SpawnEnemies(temp, gameObject);
            }
        }
    }
}
