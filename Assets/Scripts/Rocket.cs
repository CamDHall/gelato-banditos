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

            Vector3 contactPoint = coll.contacts[0].point;
            Vector3 center = coll.collider.bounds.center;

            Debug.Log(contactPoint.z + " COLL: " + center.z);

            if (contactPoint.z >= Mathf.Abs(center.z * 2) - .2f)
            {
                GameManager.Instance.Indicator("Top");
            } else if (contactPoint.z <= (center.z * 2) - .2f)
            {
                GameManager.Instance.Indicator("Bottom");
            } else if (contactPoint.y < center.y - coll.collider.bounds.extents.y)
            {
                GameManager.Instance.Indicator("Bottom");
            } else if (contactPoint.y > center.y + coll.collider.bounds.extents.y)
            {
                GameManager.Instance.Indicator("Top");
            } else if (contactPoint.x < center.x)
            {
                GameManager.Instance.Indicator("Left");
            } else if (contactPoint.x > center.x)
            {
                GameManager.Instance.Indicator("Right");
            }

            Destroy(gameObject);
        }
    }
}
