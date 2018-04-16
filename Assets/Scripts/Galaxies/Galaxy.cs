using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GalaxyName { Starting, Juarez }
public class Galaxy : MonoBehaviour {

    public GalaxyName galaxy;
    public GameObject cluster;

    public List<GameObject> planets;

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
            planet.GetComponent<Planet>().ClearField();
        }
        //else if(galaxy == GalaxyName.Juarez)

        loadingTxt.SetActive(false);
        PlayerMovement.player.enabled = true;
        GunController.Instance.enabled = true;
	}
	
	void Update () {
		
	}
}
