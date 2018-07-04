using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterManager : SerializedMonoBehaviour {

    public PlayerData pData;

	void Start () {
        pData = DataManager.LoadCharacterData();
        Debug.Log(pData.resources.Count);
	}
	
	void Update () {
		
	}
}
