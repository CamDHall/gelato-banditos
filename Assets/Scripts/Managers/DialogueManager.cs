using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour {

    public static DialogueManager Instance;
    [HideInInspector] public Affilation currentAffil;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentAffil = GetComponent<FactionManager>().factionAffil;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Character")
        {
            if(CharacterManager.Instance.pData.standings[currentAffil] <= 0)
            {
                CharacterUI.Instance.GelatoRequestScreen();
            } else
            {
                Debug.Log("woops");
            }
        }
    }
}
