using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour {

    public GameObject bullet;

    bool pressed = false;
    float fire_cooldown = 0;

    public GameObject container;

    void Update () {
        if(Input.GetAxis("Fire") != 0)
        {
            if (!pressed && fire_cooldown < Time.time)
            {
                Instantiate(bullet, transform.position + (transform.forward * 2), Quaternion.identity);
                fire_cooldown = Time.time + 0.1f;
            }
            pressed = true;
        } else
        {
            pressed = false;
        }
	}
}
