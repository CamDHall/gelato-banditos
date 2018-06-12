using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public GameObject astro;
	// Use this for initialization
	void Start () {
		for(int i = 0; i < 100; i++)
        {
            Vector3 pos = PlayerMovement.player.transform.position + Random.insideUnitSphere * 5000;

            Instantiate(astro, pos, Quaternion.identity);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
