using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroSpawner : MonoBehaviour {

    public static AstroSpawner Instance;

    public GameObject prefab_cluster;
    public GameObject loadingTxt;

    // Use dictionary so I can add health later
    public Dictionary<GameObject, int> astroids = new Dictionary<GameObject, int>();

    public int size; // Number of chunks
    public int col_size; // Size of coliders

    private void Awake()
    {
        Instance = this;
        loadingTxt.SetActive(true);
    }

    void Start () {
        StartCoroutine("InitSpawn");
    }

    IEnumerator InitSpawn()
    {
        for (int z = 0; z < size; z++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    // Set position of cluster in 3D grid
                    Vector3 Pos = new Vector3((((-size / 2) + x) * col_size) + x, (((-size / 2) + y) * col_size) + y, (((-size / 2) + z) * col_size) + z);
                    GameObject quad = Instantiate(prefab_cluster, Pos, Quaternion.identity, transform);

                    quad.GetComponent<Cluster>().Populate();

                    FieldManager.Instance.inactiveClusters.Add(quad.transform.position, quad);
                    quad.SetActive(false);

                    yield return new WaitForEndOfFrame();
                }
            }
        }

        // Set scene to play after all the spawning is finished
        loadingTxt.SetActive(false);
        GameManager.Instance.StartGame();
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
