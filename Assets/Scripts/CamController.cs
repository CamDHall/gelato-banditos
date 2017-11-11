using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

    private void FixedUpdate()
    {
        Vector3 Pos = new Vector3(PlayerMovement.player.transform.position.x, PlayerMovement.player.transform.position.y,
            PlayerMovement.player.transform.position.z - 10);
        transform.position = Pos;
    }
}
