using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Text coneCount;

	void Start () {
		
	}
	
	void Update () {
        coneCount.text = "x" + GameManager.Instance.cones.Count;
	}
}
