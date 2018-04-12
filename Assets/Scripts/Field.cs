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
    protected List<GameObject> field = new List<GameObject>();

    public GameObject astroid, bigAstro;

    protected float size;

    protected int numAstroids;

    protected float playerDist;
    protected float astroSize = 0;
    protected Vector3 Pos;

    public List<GameObject> enemies = new List<GameObject>();

    // Different clusters will/had different algorithims
    public virtual void Populate()
    {
        populated = true;
    }
}
