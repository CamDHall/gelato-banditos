using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    Rigidbody rb;
    float speed;
    public float bullet_speed;

	void Awake () {
        rb = GetComponent<Rigidbody>();
        speed = bullet_speed + PlayerMovement.player.acceleration;
        Destroy(gameObject, 15);
    }
	
	void FixedUpdate () {
        rb.MovePosition(transform.position + (Camera.main.transform.forward * speed));
	}

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "Astro")
        {
            AstroSpawner.Instance.ReceiveDamage(coll.gameObject);
        }
        if(coll.gameObject.tag == "IceCream")
        {
            GameManager.Instance.score++;
        }
        if(coll.gameObject.tag == "Bandito")
        {
            Destroy(coll.gameObject);
            Debug.Log("BANDITO");
            GameManager.Instance.score++;
        }
        Destroy(gameObject);
    }
}
