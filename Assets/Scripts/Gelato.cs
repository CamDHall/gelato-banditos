using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gelato : MonoBehaviour {

    bool moving = true;
    public float speed, scaleSpeed;

    [HideInInspector] public bool launched = false;
    [HideInInspector] public Vector3 dir;
    [HideInInspector] public Transform target;
	
	void Update () {
        if(moving)
            transform.position = Vector3.Lerp(transform.position, PlayerMovement.player.transform.position, Time.deltaTime * 10);

        if(launched)
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
            if(transform.localScale.x < 600)
            {
                transform.localScale += new Vector3(Time.deltaTime * scaleSpeed, Time.deltaTime * scaleSpeed, Time.deltaTime * scaleSpeed);
            } else
            {
                Destroy(gameObject);
            }
        }
	}

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player" && moving)
        {
            moving = false;
            Place();
        }
        else
        {
            if (coll.gameObject.tag == "Bandito" && launched)
            {
                coll.transform.parent.GetComponent<AsteroidField>().enemies.Remove(coll.gameObject);
                //BanditoSpawner.Instance.enemies.Remove(coll.gameObject);
                GameManager.Instance.friends.Add(coll.gameObject);

                coll.gameObject.GetComponent<StateController>().isFriend = true;
                coll.gameObject.GetComponentInChildren<Light>().color = Color.white;
                Destroy(gameObject);
            }
            if (coll.gameObject.tag == "Astro")
            {
                Destroy(gameObject);
            }
        }
    }

    void Place()
    {
        transform.SetParent(PlayerMovement.player.gelatoContainer);
        transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        transform.position = PlayerMovement.player.gelatoContainer.position;

        GameManager.Instance.cones.Enqueue(gameObject);
    }
}
