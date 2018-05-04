﻿using System.Collections;
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
        if (coll.gameObject.tag == "Player")
        {
            PlayerMovement.player.TakeDamge(5);
            CamController.Instance.ShakeCamera(transform.position);
        }
        else if(coll.gameObject.tag != "SpaceStation")
        {
            Debug.Log(coll.gameObject);
            Destroy(coll.gameObject);
        }

        Destroy(gameObject);
    }
}
