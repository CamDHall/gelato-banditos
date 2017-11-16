using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public GameObject bullet;

    GameObject container;

    bool pressed = false;
    float fire_cooldown = 0;

	void Start () {
        container = GameObject.Find("Bullets");
	}
	
	void Update () {
        if(Input.GetAxis("Fire") != 0)
        {
            if (!pressed && fire_cooldown < Time.time)
            {
                Instantiate(bullet, transform.position + (transform.forward * 2), Quaternion.identity, container.transform);
                fire_cooldown = Time.time + 0.1f;
            }
            pressed = true;
        } else
        {
            pressed = false;
        }
	}
}
