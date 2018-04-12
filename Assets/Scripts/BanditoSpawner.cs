using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team { Enemy, Friend };
public class BanditoSpawner : MonoBehaviour {

    public static BanditoSpawner Instance;
    public GameObject bandito;

    public Dictionary<Vector3, List<GameObject>> _enemies = new Dictionary<Vector3, List<GameObject>>();

	void Awake () {
        Instance = this;
	}

    public void SpawnEnemies(GameObject astro, GameObject quad)
    {
        Field field = quad.GetComponent<Field>();

        // In the future I want to randomize their local position and have posse's of bandits
        //int num = Random.Range(1, 3);
        Vector3 Pos = new Vector3((astro.transform.localScale.x + 2), (astro.transform.localScale.y + 2), 
            astro.transform.localScale.z + 2);

        for(int i = 0; i < 1; i++)
        {
            GameObject temp = Instantiate(bandito);
            temp.transform.parent = astro.transform;
            temp.transform.localPosition = Pos;
            temp.transform.SetParent(quad.transform);

            field.enemies.Add(temp);
            temp.SetActive(false);
        }
    }
}
