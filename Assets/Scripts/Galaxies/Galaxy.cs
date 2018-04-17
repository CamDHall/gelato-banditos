using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GalaxyName { Starting, Juarez }
public class Galaxy : MonoBehaviour {

    public GalaxyName galaxy;
    public GameObject cluster;

    public List<GameObject> planets;
    public List<GameObject> spaceStations;

    public GameObject loadingTxt;

	public virtual void Start () {
        loadingTxt.SetActive(true);
        GunController.Instance.enabled = false;
        PlayerMovement.player.enabled = false;

        GameObject temp = Instantiate(cluster);
        temp.transform.SetParent(transform);
        temp.GetComponent<Cluster>().Populate();

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
