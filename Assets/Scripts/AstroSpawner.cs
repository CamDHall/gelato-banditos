using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroSpawner : MonoBehaviour {

    public static AstroSpawner Instance;

    public GameObject prefab_quadrant, prefab_cluster;

    List<GameObject> quadrants = new List<GameObject>();
    List<GameObject> clusters = new List<GameObject>();
    public Dictionary<GameObject, int> astroids = new Dictionary<GameObject, int>();

    public int size;
    public int col_size;

	void Start () {
        Instance = this;

        for (int z = 0; z < size; z++) {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if ((x == 5 && y == 6 && z == 5) ||Random.Range(0, 100) > 95)
                    {
                        // cluster
                        Vector3 Pos = new Vector3((((-size / 2) + x) * col_size) + x, (((-size / 2) + y) * col_size) + y, (((-size / 2) + z) * col_size) + z);
                        GameObject quad = Instantiate(prefab_cluster, Pos, Quaternion.identity, transform);
                        quadrants.Add(quad);
                    }
                    else
                    {
                        Vector3 Pos = new Vector3((((-size / 2) + x) * col_size) + x, (((-size / 2) + y) * col_size) + y, (((-size / 2) + z) * col_size) + z);
                        GameObject quad = Instantiate(prefab_quadrant, Pos, Quaternion.identity, transform);
                        quadrants.Add(quad);
                    }
                }
            }
        }
	}
	
	public void ReceiveDamage(GameObject astro)
    {
        if (astroids[astro] > 1)
        {
            astroids[astro] -= 1;
        } else
        {
            Destroy(astro);
            GameManager.Instance.score++;
            //ps.Play();
        }
    }
}
