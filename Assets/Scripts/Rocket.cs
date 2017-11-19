using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rb;
    float speed;
    public float rocket_speed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        speed = rocket_speed;
        Destroy(gameObject, 10);
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + (transform.forward * speed));
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Astro")
        {
            Destroy(gameObject);
        }
        if(coll.gameObject.tag == "Player")
        {
            PlayerMovement.player.TakeDamge(5);
        }
    }
}
