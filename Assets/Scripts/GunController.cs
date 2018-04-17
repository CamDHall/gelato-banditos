using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour {

    public static GunController Instance;
    bool pressed = false;
    public float fire_cooldown = 0;

    float timer;

    public Transform container;
    public Transform origin;
    public GameObject crosshair;
    public GameObject laser;
    
    public LineRenderer prefab_laser;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        timer = 0;
    }

    void Update () {
        if(Input.GetAxis("Fire") != 0 && !GelatoCanon.Instance.holding)
        {
            if (!pressed && timer < Time.timeSinceLevelLoad)
            {
                AudioManager.Instance.Laser();
                LineRenderer laser = Instantiate(prefab_laser);
                laser.transform.SetParent(transform);
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;
                if(Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    AudioManager.Instance.ChangeVolume(Vector3.Distance(hit.transform.position, transform.position));
                    laser.SetPosition(0, origin.position);
                    laser.SetPosition(1, hit.point);
                    if (hit.transform.tag == "Bandito")
                    {
                        AudioManager.Instance.BanditoSplat();
                        hit.transform.GetComponent<StateController>().Die();
                        GameManager.Instance.score += 5;
                    }
                    else
                    {
                        if(hit.transform.tag == "Astro")
                        {
                            GameManager.Instance.score++;
                            AudioManager.Instance.AstroCrack();
                        }

                        if (hit.transform.tag == "Astro")
                        {
                            Destroy(hit.transform.gameObject, Time.deltaTime * 5);
                            Utilts.GetResources(hit.transform.gameObject);
                        }
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
