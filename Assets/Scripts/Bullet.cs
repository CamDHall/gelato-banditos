using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    Rigidbody rb;
    float speed;
    Vector3 direction;
    public float bullet_speed;

	void Awake () {
        rb = GetComponent<Rigidbody>();
        speed = bullet_speed + PlayerMovement.player.acceleration;
        direction = PlayerMovement.player.transform.forward;
        Destroy(gameObject, 10);
    }
	
	void FixedUpdate () {
        rb.MovePosition(transform.position + (direction * speed));
	}

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "Astro")
        {
            AstroSpawner.Instance.ReceiveDamage(coll.gameObject);
            Destroy(gameObject);
        }
        if(coll.gameObject.tag == "IceCream")
        {
            GameManager.Instance.score++;
        }
    }
}
