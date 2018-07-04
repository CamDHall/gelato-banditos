using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterManager : SerializedMonoBehaviour {

    public static CharacterManager Instance;

    public CharacterMovement character;
    public PlayerData pData;

    private void Awake()
    {
        Instance = this;
    }

    void Start () {
        pData = DataManager.LoadCharacterData();
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "SceneTrigger")
        {
            CharacterUI.Instance.LeaveScene();
        }
    }
}
