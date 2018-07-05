using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Secretariat : MonoBehaviour {

    public Affilation affil;
    public GameObject greeting;


	void Start () {
        greeting.SetActive(false);
	}
	
	void Update () {
		
	}

    void Greet()
    {
        CharacterManager.Instance.character.enabled = false;
        greeting.SetActive(true);
        CinematicUI.Instance.SetupStore();
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "Character" && CharacterManager.Instance.pData.standings[affil] > 0)
        {
            Greet();
        } else
        {
            Debug.Log("GUARDS!");
        }
    }
}
