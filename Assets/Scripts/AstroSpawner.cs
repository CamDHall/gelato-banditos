using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroSpawner : MonoBehaviour {

    public GalaxyName galaxy;
    public static AstroSpawner Instance;

    public GameObject gal_Starting, gal_Juarez;
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
                    GameObject quad;

                    if (galaxy == GalaxyName.Starting)
                    {
                        quad = Instantiate(gal_Starting, transform);
                        quad.GetComponent<Cluster>().Populate();
                    } else if(galaxy == GalaxyName.Juarez)
                    {
                        quad = Instantiate(gal_Juarez, transform);
                        
                    }

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
        }
    }
}
