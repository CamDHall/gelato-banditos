using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GalaxyName { Starting, Juarez }
public class Galaxy : MonoBehaviour {

    public GalaxyName galaxy;
    public GameObject cluster;
    public GameObject loadingTxt;

	public virtual void Start () {
        
        GameObject temp = Instantiate(cluster);
        temp.transform.SetParent(transform);
        if(galaxy == GalaxyName.Starting) temp.GetComponent<Cluster>().Populate();
        //else if(galaxy == GalaxyName.Juarez)
        loadingTxt.SetActive(false);
        PlayerMovement.player.enabled = true;
	}
	
	void Update () {
		
	}
}
