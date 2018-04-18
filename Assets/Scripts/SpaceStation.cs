using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Affilation { ChihuahuaFederation }
public class SpaceStation : MonoBehaviour {

    public Affilation spaceStation_affil;
    public List<GameObject> weapons;
    StationController sc;

	void Start () {
        sc = GetComponent<StationController>();
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            if (!sc.aiActive)
            {
                GameManager.Instance.nearestStation = gameObject;
                sc.aiActive = true;
            }
        }
    }
}
