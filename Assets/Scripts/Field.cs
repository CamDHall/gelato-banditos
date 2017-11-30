using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour {

    public int numLow, numHigh, spawnChance;

    public bool populated = false;
    public bool turnedOff = false;
    protected List<GameObject> field = new List<GameObject>();

    public GameObject astroid, bigAstro;
    protected int size;

    protected int numAstroids;

    protected float playerDist;
    protected float camDepth;
    protected BoxCollider box;
    protected float astroSize = 0;
    protected Vector3 Pos;

    public List<GameObject> enemies = new List<GameObject>();

    public void TurnOff()
    {
        turnedOff = true;

        GameObject[] temp = field.ToArray();

        foreach (GameObject obj in temp)
        {
            if (obj == null)
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

        foreach (GameObject enemy in enemies)
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

        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(true);
        }
    }

    public virtual void Populate()
    {
        populated = true;
    }
}
