using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour {

    public static GunController Instance;
    bool pressed = false;
    public float fire_cooldown = 0;
    public int stationRange;

    float timer;
    bool canHitStation;

    public Transform container;
    public Transform origin;
    public GameObject crosshair;
    
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
        if(Input.GetAxis("Fire") != 0)
        {
            if (!pressed && timer < Time.timeSinceLevelLoad)
            {
                AudioManager.Instance.Laser();
                LineRenderer laser = Instantiate(prefab_laser);
                laser.transform.SetParent(transform);
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit))
                {
                    string hTag = hit.transform.tag;

                    if (hTag == "SpaceStation")
                    {
                        if (Vector3.Distance(transform.position, hit.transform.position) < stationRange)
                        {
                            canHitStation = true;
                        } else
                        {
                            canHitStation = false;
                        }
                    }

                    AudioManager.Instance.ChangeVolume(Vector3.Distance(hit.transform.position, transform.position));
                    laser.SetPosition(0, origin.position);
                    laser.SetPosition(1, hit.point);
                    if (hTag== "Bandito")
                    {
                        AudioManager.Instance.BanditoSplat();
                        hit.transform.GetComponent<Flocking>().Death();
                    }
                    else if(!(hTag == "SpaceStation" && !canHitStation))
                    {
                        if (hTag == "Astro" || hTag == "StationWeapons")
                        {
                            AudioManager.Instance.AstroCrack();
                        }

                        IDamageable Idamage = hit.transform.gameObject.GetComponent<IDamageable>();
                        
                        if(Idamage == null && hit.transform.parent != null)
                        { 
                            Idamage = hit.transform.parent.gameObject.GetComponent<IDamageable>();
                        }

                        if(Idamage!= null)
                        {
                            Idamage.TakeDamage(1);
                        }

                        if (hTag == "Astro")
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
