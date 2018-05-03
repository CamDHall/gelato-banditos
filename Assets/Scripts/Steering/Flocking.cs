using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : MonoBehaviour {
    public float neighborDist;
    float speed;
    FlockingParent fp;
    Vector3 waypoint;

    [HideInInspector] public bool leaderDead = false;

	void Start () {
        speed = Random.Range(5, 120) * Time.deltaTime;
        fp = transform.parent.transform.GetComponent<FlockingParent>();
	}
	
	void Update () {

        transform.LookAt(PlayerMovement.player.transform);

        if (!leaderDead)
        {
            transform.position += transform.forward * speed;
        } else
        {
            if(waypoint == Vector3.zero || Vector3.Distance(waypoint, transform.position) < 1f)
            {
                waypoint = (Random.insideUnitSphere * 30) + transform.position;
            }

            transform.position = Vector3.Lerp(transform.position, waypoint, Time.deltaTime);
        }
	}

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Bandito") return;

        if(coll.gameObject.tag == "Player")
        {
            PlayerMovement.player.TakeDamge(0.1f);
        }

        fp.flock.Remove(gameObject);
        Destroy(gameObject);
    }
}
