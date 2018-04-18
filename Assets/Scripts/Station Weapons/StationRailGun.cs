using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationRailGun : MonoBehaviour {

    public GameObject rail;
    public float firingRate;
    float timer;

	void Start () {
        timer = Time.timeSinceLevelLoad;
	}
	
	void Update () {
        transform.LookAt(PlayerMovement.player.transform);

		if(timer < Time.timeSinceLevelLoad)
        {
            GameObject temp = Instantiate(rail, transform.parent);
            temp.transform.position = transform.position + transform.forward * 50;
            temp.transform.rotation = transform.rotation;

            timer = Time.timeSinceLevelLoad + firingRate;
        }
	}
}
