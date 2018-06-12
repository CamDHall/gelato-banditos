using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GalaxyName { Starting, Juarez }
public class Galaxy : MonoBehaviour {

    public GalaxyName galaxy;
    public GameObject cluster;
    public int numClusters = 0;
    public List<GameObject> planets;
    public List<GameObject> spaceStations;

    Vector3 newPos;
    float size;
    int counter;
    Vector3[] positions;

    public GameObject loadingTxt;

	public virtual void Start () {
        loadingTxt.SetActive(true);
        GunController.Instance.enabled = false;
        PlayerMovement.player.enabled = false;

        positions = new Vector3[numClusters];

        size = cluster.GetComponent<Cluster>().size / 2.5f;

        for (int i = 0; i < numClusters; i++)
        {
            if (i == 0)
            {
                positions[0] = new Vector3(Random.Range(-size, size), Random.Range(-size, size), Random.Range(-size, size));
            } else {

                counter = 0;
                newPos = new Vector3(Random.Range(-size, size), Random.Range(-size, size), Random.Range(-size, size));
                foreach (Vector3 pos in positions)
                {
                    if(Vector3.Distance(pos, newPos) < 1000)
                    {
                        newPos = new Vector3(Random.Range(-size, size), Random.Range(-size, size), Random.Range(-size, size));
                    }
                }
            }

            GameObject temp = Instantiate(cluster, newPos, Quaternion.identity, transform);
            temp.GetComponent<Cluster>().Populate();
        }

        foreach(GameObject planet in planets)
        {
            Utilts.ClearField(planet.transform.position, 1500);
        }

        foreach (GameObject station in spaceStations)
        {
            Utilts.ClearField(station.transform.position, 750);
        }
        //else if(galaxy == GalaxyName.Juarez)

        loadingTxt.SetActive(false);
        PlayerMovement.player.enabled = true;
        GunController.Instance.enabled = true;
	}
	
	void Update () {
		
	}
}
