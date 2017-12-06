﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team { Enemy, Friend };
public class BanditoSpawner : MonoBehaviour {

    public static BanditoSpawner Instance;
    public GameObject bandito;

    List<GameObject> enemies = new List<GameObject>();
    List<GameObject> friends = new List<GameObject>();

	void Start () {
        Instance = this;
	}

    public void SpawnEnemies(GameObject astro, GameObject quad)
    {
        SphereCollider sc = astro.GetComponent<SphereCollider>();
        Cluster cluster = quad.GetComponent<Cluster>();

        //int num = Random.Range(1, 3);
        Vector3 Pos = new Vector3((sc.radius / 2), (sc.radius / 2), (sc.radius / 2));

        for(int i = 0; i < 1; i++)
        {
            GameObject temp = Instantiate(bandito);
            temp.transform.parent = astro.transform;
            temp.transform.localPosition = Pos;
            temp.transform.SetParent(transform);
            enemies.Add(temp);
            cluster.enemies.Add(temp);
        }



    }
}
