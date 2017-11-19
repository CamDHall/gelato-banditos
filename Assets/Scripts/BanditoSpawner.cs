using System.Collections;
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

    public void SpawnEnemies(GameObject astro)
    {
        BoxCollider bc = astro.GetComponent<BoxCollider>();
        int num = Random.Range(1, 3);
        Vector3 Pos = new Vector3((bc.size.x / 2), (bc.size.y / 2), (bc.size.z / 2));

        for(int i = 0; i < num; i++)
        {
            GameObject temp = Instantiate(bandito);
            temp.transform.parent = astro.transform;
            temp.transform.localPosition = Pos;
            temp.transform.parent = null;
            enemies.Add(temp);
        }

    }
}
