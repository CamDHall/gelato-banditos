using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour {

    public GameObject bullet;

    bool pressed = false;
    float fire_cooldown = 0;

    public GameObject container;
    public GameObject crosshair;

    void Update () {
        if(Input.GetAxis("Fire") != 0)
        {
            if (!pressed && fire_cooldown < Time.time)
            {
                //Vector3 Pos = new Vector3(crosshair.transform.position.x, crosshair.transform.position.y, crosshair.transform.position.z) + transform.forward;
                GameObject temp = Instantiate(bullet);
                temp.transform.parent = container.transform;
                temp.transform.position = transform.position + (transform.forward * 6);

                fire_cooldown = Time.time + 0.1f;
            }
            pressed = true;
        } else
        {
            pressed = false;
        }
	}
}
