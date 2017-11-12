using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroSpawner : MonoBehaviour {

    public static AstroSpawner Instance;

    public GameObject prefab_quadrant;

    List<GameObject> quadrants = new List<GameObject>();

    public int size;
    public int col_size;

	void Start () {
        Instance = this;

        for (int z = 0; z < size; z++) {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    // Add one to avoid overlapping colliders
                    Vector3 Pos = new Vector3((((-size / 2) + x) * col_size) + x, (((-size / 2) + y) * col_size) + y, (((-size / 2)  + z) * col_size) + z);
                    GameObject quad = Instantiate(prefab_quadrant, Pos, Quaternion.identity, transform);
                    quadrants.Add(quad);
                }
            }
        }
	}
	
	void Update () {
		
	}
}
