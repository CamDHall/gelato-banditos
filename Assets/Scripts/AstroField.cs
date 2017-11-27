using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroField : MonoBehaviour {

    public int numLow, numHigh, spawnChance;

    public bool populated = false;
    public bool turnedOff = false;
    List<GameObject> field = new List<GameObject>();

    public GameObject astroid;
    int size;

    int numAstroids;

    BoxCollider box;
    float camDepth;

    public List<GameObject> enemies = new List<GameObject>();

    float playerDist = 0;

    private void Awake()
    {
        box = GetComponent<BoxCollider>();
        numAstroids = Random.Range(numLow, numHigh);
        size = AstroSpawner.Instance.col_size;

        camDepth = Camera.main.farClipPlane;
    }

    private void Update()
    {
        /*playerDist = Vector3.Distance(PlayerMovement.player.transform.position, box.ClosestPoint(PlayerMovement.player.transform.position));

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
        }*/
    }

    public void Populate()
    {
        populated = true;
        for (int i = 0; i < numAstroids; i++)
        {
            Vector3 Pos = new Vector3((float)Random.Range(-size/2, size/2), (float)Random.Range(-size/2, size/2), (float)Random.Range(-size/2, size/2));
            GameObject temp = Instantiate(astroid);
            temp.transform.parent = transform;
            temp.transform.localPosition = Pos;
            field.Add(temp);
            //AstroSpawner.Instance.astroids.Add(temp, Random.Range(2, 5));
            AstroSpawner.Instance.astroids.Add(temp, 1);

            if(Random.Range(0, 100) > spawnChance)
            {
                BanditoSpawner.Instance.SpawnEnemies(temp, gameObject);
            }
        }

        TurnOn();
    }

    public void TurnOff()
    {
        turnedOff = true;

        GameObject[] temp = field.ToArray();

        foreach(GameObject obj in temp)
        {
            if(obj == null)
            {
                field.Remove(obj);
            }
        }

        GameObject[] tempEnemies = enemies.ToArray();

        foreach (GameObject obj in tempEnemies)
        {
            if (obj == null)
            {
                enemies.Remove(obj);
            }
        }

        foreach (GameObject astro in field)
        {
            astro.SetActive(false);
        }

        foreach(GameObject enemy in enemies)
        {
            enemy.SetActive(false);
        }
    }

    public void TurnOn()
    {
        turnedOff = false;

        GameObject[] temp = field.ToArray();

        foreach (GameObject obj in temp)
        {
            if (obj == null)
            {
                field.Remove(obj);
            }
        }

        foreach (GameObject astro in field)
        {
            astro.SetActive(true);
        }

        foreach(GameObject enemy in enemies)
        {
            enemy.SetActive(true);
        }
    }
}
