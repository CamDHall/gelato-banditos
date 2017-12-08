using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour {

    public GameObject bullet;

    bool pressed = false;
    float fire_cooldown = 0;

    public Transform container;
    public Transform origin;
    public GameObject crosshair;
    public GameObject laser;
    
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
                laser.transform.position = transform.position;
                laser.transform.SetParent(container);
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;
                if(Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    laser.SetPosition(0, origin.position);
                    laser.SetPosition(1, hit.point);
                    Debug.Log(origin.position);

                    laser.enabled = true;
                } else
                {
                    laser.SetPosition(0, origin.position);
                    laser.SetPosition(1, ray.GetPoint(10000));
                }

                //GameObject temp = Instantiate(laser);
                //temp.transform.SetParent(container.transform);
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
