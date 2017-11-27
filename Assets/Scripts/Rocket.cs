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
        if(coll.gameObject.tag == "Player")
        {
            PlayerMovement.player.TakeDamge(1);
        } else if(coll.gameObject.tag == "Astro" || coll.gameObject.tag == "Bandito")
        {
            Destroy(coll.gameObject);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll == PlayerMovement.player.colliders[1])
        {
            GameManager.Instance.Indicator("Left");
        } else if(coll == PlayerMovement.player.colliders[2])
        {
            GameManager.Instance.Indicator("Right");
        } else if(coll == PlayerMovement.player.colliders[3])
        {
            GameManager.Instance.Indicator("Front");
        } else if(coll == PlayerMovement.player.colliders[4])
        {
            GameManager.Instance.Indicator("Back");
        } else if(coll == PlayerMovement.player.colliders[5])
        {
            GameManager.Instance.Indicator("Top");
        } else
        {
            GameManager.Instance.Indicator("Bottom");
        }
    }
}
