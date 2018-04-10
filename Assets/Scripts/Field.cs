using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Using inheretence for clusters so in the future I can have different patterns and types of asteroid fields
/// </summary>

public class Field : MonoBehaviour {

    public int numLow, numHigh, spawnChance;
    public int clustSize;

    public bool populated = false;
    public bool turnedOff = false;
    protected List<GameObject> field = new List<GameObject>();

    public GameObject astroid, bigAstro;
    protected float size;

    protected int numAstroids;

    protected float playerDist;
    protected float camDepth;
    protected float astroSize = 0;
    protected Vector3 Pos;

    public List<GameObject> enemies = new List<GameObject>();

    public void TurnOff()
    {
        turnedOff = true;

        // Arraty seems to be slightly faster than list, and the temp array doesn't have to change
        GameObject[] temp = field.ToArray();

        foreach (GameObject obj in temp)
        {
            if (obj == null)
            {
                field.Remove(obj);
            }
        }

        GameObject[] tempEnemies = enemies.ToArray();

        // Remove any enemies that have been killed
        foreach (GameObject obj in tempEnemies)
        {
            if (obj == null)
            {
                enemies.Remove(obj);
            }
        }

        // Set new astro list and enemy list
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

    // Different clusters will/had different algorithims
    public virtual void Populate()
    {
        populated = true;
    }
}
