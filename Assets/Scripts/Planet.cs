using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ClearField()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 1000, transform.forward, 1 << LayerMask.NameToLayer("Astro"));
        int count = hits.Length;

        for(int i = 0; i < count; i++)
        {
            if(hits[i].transform.tag == "Astro")
                Destroy(hits[i].transform.gameObject);
        }
    }
}
