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
    public LineRenderer prefab_laser;

    private void Start()
    {
        
    }

    void Update () {
        if(Input.GetAxis("Fire") != 0)
        {
            if (!pressed && fire_cooldown < Time.time)
            {
                LineRenderer laser = Instantiate(prefab_laser);
                RaycastHit hit;
                if(Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    laser.SetPosition(0, transform.position);
                    laser.SetPosition(1, hit.transform.position);

                    laser.enabled = true;
                } else
                {
                    laser.SetPosition(0, transform.position);
                    laser.SetPosition(1, transform.position + (transform.forward * 100));
                }
                //GameObject temp = Instantiate(bullet);
                //temp.transform.parent = container.transform;
                //temp.transform.position = transform.position + (transform.forward * 6);

                fire_cooldown = Time.time + 0.1f;
            }
            pressed = true;
        } else
        {
            pressed = false;
        }
	}
}
