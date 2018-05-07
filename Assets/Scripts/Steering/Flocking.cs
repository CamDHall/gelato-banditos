using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : MonoBehaviour {
    public float attackRate;
    public GameObject bullet;

    float speed, slothSpeed;
    Vector3 waypoint;

    GameObject target;
    float attackTimer;

    [HideInInspector] public bool leaderDead = false;
    [HideInInspector] public bool friendly = false;
    [HideInInspector] public FlockingParent fp;

    void Start () {
        speed = Random.Range(50, 80) * Time.deltaTime;
        slothSpeed = Random.Range(1, 2) * Time.deltaTime;
        fp = transform.parent.transform.GetComponent<FlockingParent>();
	}
	
	void Update () {

        if (!leaderDead && !friendly)
        {
            transform.LookAt(PlayerMovement.player.transform);
            transform.position += transform.forward * speed;
        } else if(!friendly)
        {
            if(waypoint == Vector3.zero)
            {
                waypoint = (Random.insideUnitSphere * 30) + transform.position;
                transform.LookAt(waypoint);
            }

            transform.position += (transform.forward * slothSpeed);
        }

        if(friendly)
        {
            target = Utilts.FindTarget(gameObject.transform);
            transform.LookAt(target.transform);

            if (attackTimer < Time.timeSinceLevelLoad)
            {
                attackTimer = Time.timeSinceLevelLoad + attackRate;
                Instantiate(bullet, transform.position + transform.forward * 3, transform.rotation);
            }
        }
	}

    public void Death()
    {
        if(gameObject.name.Contains("Leader"))
        {
            fp.RemoveLeader();
        }

        fp.flock.Remove(gameObject);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Bandito" || (!friendly && coll.gameObject.tag == "Cone"))
            return;

        if(coll.gameObject.tag == "Player")
        {
            PlayerMovement.player.TakeDamge(0.5f);
        } else if(coll.gameObject.name.Contains("Leader"))
        {
            fp.flock.Remove(gameObject);
            fp.RemoveLeader();
            Destroy(gameObject);
        }

        fp.flock.Remove(gameObject);
        Destroy(gameObject);
    }
}
