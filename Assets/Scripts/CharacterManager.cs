using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {

    public static CharacterManager Instance;

    public int copper, iron;
    public Dictionary<Affilation, int> standings = new Dictionary<Affilation, int>();

    void Awake()
    {
        Instance = this;
    }

	void Start () {
        standings.Add(Affilation.ChihuahuaFederation, 0);
	}
	
	// Update is called once per frame
	void Update () {
       
	}
}
