using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Projectile : MonoBehaviour {

    public float speed;
    Rigidbody rb;
    BoxCollider bc;
    public Transform _parent;
    float timer;

	void Start () {
        bc = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 5);
        timer = Time.timeSinceLevelLoad + (Time.deltaTime * 2);
	}
	
	void FixedUpdate () {
        if(timer < Time.timeSinceLevelLoad)
        {
            bc.enabled = true;
        }
        rb.MovePosition(transform.position + (transform.forward * speed));
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.transform == _parent || coll.transform.tag == "Cluster")
        {
            return;
        }
        if (coll.gameObject.tag == "Player")
        {
            PlayerMovement.player.TakeDamge(5);
            CamController.Instance.ShakeCamera(transform.position);
        }
        else if (coll.gameObject.tag == "SpaceStation" || coll.gameObject.tag == "StationWeapon")
        {
            coll.gameObject.GetComponent<IDamageable>().TakeDamage(5);
        }
        else
        {
            Destroy(coll.gameObject);
        }

        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider coll)
    {
        if (coll.transform == _parent || coll.transform.tag == "Cluster" || coll.transform.tag == "SpaceStation") {
            return;
        }
        if (coll.gameObject.tag == "Player")
        {
            PlayerMovement.player.TakeDamge(5);
            CamController.Instance.ShakeCamera(transform.position);
        }
        else if(coll.gameObject.tag == "SpaceStation" || coll.gameObject.tag == "StationWeapon")
        {
            coll.gameObject.GetComponent<IDamageable>().TakeDamage(5);
        } else
        {
            Destroy(coll.gameObject);
        }

        Destroy(gameObject);
    }
}
