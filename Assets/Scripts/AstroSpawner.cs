using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroSpawner : MonoBehaviour {

    public static AstroSpawner Instance;

    public GameObject prefab_cluster;
    
    // Use dictionary so I can add health later
    public Dictionary<GameObject, int> astroids = new Dictionary<GameObject, int>();

    public int size; // Number of chunks
    public int col_size; // Size of coliders

    private void Awake()
    {
        Instance = this;
    }

    void Start () {
        for (int z = 0; z < size; z++) {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    // Set position of cluster in 3D grid
                    Vector3 Pos = new Vector3((((-size / 2) + x) * col_size) + x, (((-size / 2) + y) * col_size) + y, (((-size / 2) + z) * col_size) + z);
                    GameObject quad = Instantiate(prefab_cluster, Pos, Quaternion.identity, transform);
                    FieldManager.Instance.clusters.Add(quad.transform, quad);
                    FieldManager.Instance.activeClust.Add(quad);

                    if((x == 5 && y == 5 && z == 5))
                    {
                        // Make sure the first cluster is populated 
                        quad.GetComponent<Cluster>().Populate();
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
        }
    }
}
