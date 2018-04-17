using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {

    public static CharacterManager Instance;

    public int copper, iron;

    void Awake()
    {
        Instance = this;
    }

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(copper);
	}
}
