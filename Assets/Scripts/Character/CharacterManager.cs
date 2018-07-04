using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterManager : SerializedMonoBehaviour {

    public static CharacterManager Instance;

    public CharacterMovement character;
    public PlayerData pData;

    float dPadHorizontal = 0;
    bool horizontalPadInUse = false;

    private void Awake()
    {
        pData = DataManager.LoadCharacterData();
        Instance = this;

        pData.gelatoContainer = GameObject.FindGameObjectWithTag("GelatoContainer");
        pData.flavors = pData.gelatoContainer.GetComponents<Flavor>();
    }

    void Start () {

        GelatoCanon.Instance.UpdateCounter(0, true);
	}
	
	void Update () {
        dPadHorizontal = Input.GetAxis("DpadHorizontal");
        bool tempPad = Input.GetKeyDown(KeyCode.RightArrow);

        if (!horizontalPadInUse)
        {
            if (dPadHorizontal > 0 || tempPad)
            {
                GelatoCanon.Instance.UpdateCounter(1, false);
                horizontalPadInUse = true;
            }
            else if (dPadHorizontal < 0)
            {
                GelatoCanon.Instance.UpdateCounter(-1, false);
                horizontalPadInUse = true;
            }
        }
        else if (dPadHorizontal == 0)
        {
            horizontalPadInUse = false;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "SceneTrigger")
        {
            CharacterUI.Instance.LeaveScene();
        }
    }
}
