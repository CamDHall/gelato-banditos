using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour {

    public GameObject bullet;

    bool pressed = false;
    public float fire_cooldown = 0;

    float timer;

    public Transform container;
    public Transform origin;
    public GameObject crosshair;
    public GameObject laser;
    
    public LineRenderer prefab_laser;

    private void Start()
    {
        timer = 0;
    }

    void Update () {
        if(Input.GetAxis("Fire") != 0)
        {
            if (!pressed && timer < Time.timeSinceLevelLoad)
            {
                LineRenderer laser = Instantiate(prefab_laser);
                laser.transform.SetParent(transform);
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;
                if(Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    laser.SetPosition(0, origin.position);
                    laser.SetPosition(1, hit.point);
                    if (hit.transform.tag == "Bandito")
                    {
                        hit.transform.GetComponent<StateController>().Die();
                    }
                    else
                    {
                        Destroy(hit.transform.gameObject, Time.deltaTime * 5);
                    }
                    laser.enabled = true;
                } else
                {
                    laser.SetPosition(0, origin.position);
                    laser.SetPosition(1, ray.GetPoint(10000));
                }

                timer = Time.timeSinceLevelLoad + fire_cooldown;
            }
            pressed = true;
        } else
        {
            pressed = false;
        }
	}
}
