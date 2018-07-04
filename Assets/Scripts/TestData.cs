using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class TestData : SerializedMonoBehaviour {

    [OdinSerialize] public int num = 5;
    [OdinSerialize] public string test = "TEST";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
