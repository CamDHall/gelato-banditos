using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juarez : Galaxy {

    public GameObject planet;

	public override void Start() {
        base.Start();
        Instantiate(planet);
	}
	
	void Update () {
		
	}
}
