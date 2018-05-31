using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed;
    Rigidbody rb;
	void Start () {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 5);
	}
	
	void FixedUpdate () {
        rb.MovePosition(transform.position + (transform.forward * speed));
    }

    private void OnCollisionEnter(Collision coll)
    {
        Debug.Log(coll.gameObject.name);
        if (coll.gameObject.tag == "Player")
        {
            PlayerMovement.player.TakeDamge(5);
            CamController.Instance.ShakeCamera(transform.position);
        }
        else if(coll.gameObject.tag == "SpaceStation")
        {
            coll.gameObject.GetComponent<IDamageable>().TakeDamage(5);
        } else
        {
            Destroy(coll.gameObject);
        }

        Destroy(gameObject);
    }
}
