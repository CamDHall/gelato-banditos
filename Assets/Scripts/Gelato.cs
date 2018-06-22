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

    private void Awake()
    {
        bc = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        launched = false;
        bc.enabled = false;
        scaleSpeed *= Time.deltaTime;
        speed *= Time.deltaTime;
    }

    void Update () {
        if (launched)
        {
            if (transform.parent != null) transform.SetParent(null);

            if(target !=null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed);
            } else
            {
                if(!markedForDeath)
                {
                    markedForDeath = true;
                    Destroy(gameObject, 0.5f);
                }
                transform.position += (transform.forward * (speed + PlayerMovement.player.currentSpeed));
            }
        }
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
