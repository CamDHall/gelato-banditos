using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gelato : MonoBehaviour {
    public Flavors flavor;
    public float speed, scaleSpeed;

    [HideInInspector] public bool launched = false;
    [HideInInspector] public Vector3 dir;
    [HideInInspector] public Transform target;
    [HideInInspector] public BoxCollider bc;

    bool markedForDeath = false;
    private void Start()
    {
        launched = false;
        bc = GetComponent<BoxCollider>();
        bc.enabled = false;
    }

    void Update () {
        if (launched)
        {
            if (transform.parent != null) transform.SetParent(null);

            transform.position += (transform.forward * speed);
            if (!markedForDeath)
            {
                speed += PlayerMovement.player.acceleration;
                Destroy(gameObject, 3);
                markedForDeath = true;
            }

            if (transform.localScale.x < 50)
            {
                transform.localScale += new Vector3(Time.deltaTime * scaleSpeed, Time.deltaTime * scaleSpeed, Time.deltaTime * scaleSpeed);

            }
        }

            /*if (launched)
            {
                if(transform.parent != null)
                {
                    transform.SetParent(null);
                }

                if (target != null)
                {
                    transform.position = Vector3.Lerp(transform.position, target.transform.position, 0.15f);
                }
                else
                {

                    transform.position += (dir * (PlayerMovement.player.acceleration + speed));
                }
                if(transform.localScale.x < 500)
                {
                    transform.localScale += new Vector3(Time.deltaTime * scaleSpeed, Time.deltaTime * scaleSpeed, Time.deltaTime * scaleSpeed);
                } else
                {
                    Destroy(gameObject);
                }
            }*/
        }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Bandito")
        {
            coll.transform.parent.GetComponent<FlockingParent>().flock.Remove(coll.gameObject);
            GameManager.Instance.friends.Add(coll.gameObject);

            coll.gameObject.GetComponent<MeshRenderer>().material = GelatoCanon.Instance.mat;
            coll.gameObject.name = "Friend";
            coll.gameObject.GetComponent<Flocking>().friendly = true;
            Destroy(gameObject);
        }
        if (coll.gameObject.tag == "Astro")
        {
            Destroy(gameObject);
        }
    }

    /*void Place()
    {
        transform.SetParent(PlayerMovement.player.gelatoContainer);
        transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        transform.position = PlayerMovement.player.gelatoContainer.position;

        PlayerInventory.Instance.cones.Remove(gameObject);
    }*/
}
