using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Affilation { ChihuahuaFederation }
public class SpaceStation : MonoBehaviour {

    public Affilation spaceStation_affil;
    public List<GameObject> weapons;

	void Start () {
		
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            int value = CharacterManager.Instance.standings[spaceStation_affil];

            if (value < 0)
            {
                Debug.Log("ATTACK");
            } else if(value == 0)
            {
                Debug.Log("Approach");
            } else
            {
                Debug.Log("DO NOTHING");
            }
        }
    }
}
