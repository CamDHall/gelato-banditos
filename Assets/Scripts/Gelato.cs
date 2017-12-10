using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gelato : MonoBehaviour {

    bool moving = true;
    public bool launched = false;
    public Transform dir;

	void Start () {
		
	}
	
	void Update () {
        if(moving)
            transform.position = Vector3.Lerp(transform.position, PlayerMovement.player.transform.position, Time.deltaTime * 10);

        if(launched)
        {
            Debug.Log(dir);
            transform.Translate(dir.forward);
        }
	}

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player" && moving)
        {
            moving = false;
            Place();
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
