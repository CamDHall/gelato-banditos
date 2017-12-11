using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gelato : MonoBehaviour {

    bool moving = true;
    public float speed, scaleSpeed;

    [HideInInspector] public bool launched = false;
    [HideInInspector] public Vector3 dir;
	
	void Update () {
        if(moving)
            transform.position = Vector3.Lerp(transform.position, PlayerMovement.player.transform.position, Time.deltaTime * 10);

        if(launched)
        {
            if(transform.parent != null)
            {
                transform.SetParent(null);
            }

            transform.position += (dir * (PlayerMovement.player.acceleration + speed));
            if(transform.localScale.x < 500)
            {
                transform.localScale += new Vector3(Time.deltaTime * scaleSpeed, Time.deltaTime * scaleSpeed, Time.deltaTime * scaleSpeed);
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

        if(coll.gameObject.tag == "Bandito" && launched)
        {
            BanditoSpawner.Instance.enemies.Remove(coll.gameObject);
            BanditoSpawner.Instance.friends.Add(coll.gameObject);

            coll.gameObject.GetComponent<StateController>().isFriend = true;
            Destroy(gameObject);

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
