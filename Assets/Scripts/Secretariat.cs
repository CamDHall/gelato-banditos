using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Secretariat : SerializedMonoBehaviour {

    public Faction faction;
    public GameObject greeting;
    public Dictionary<string, List<ChatMessage>> messages;
    public StoreItemInfo inventory;

    private void Awake()
    {
        foreach (string key in messages.Keys)
        {
            foreach (ChatMessage message in messages[key])
            {
                message.GenerateFinalMessage(true);
            }
        }
    }

    void Start () {
        greeting.SetActive(false);
	}

    void Greet()
    {
        CharacterManager.Instance.character.enabled = false;
        greeting.SetActive(true);
        CinematicUI.Instance.SetupStore(this);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "Character" && CharacterManager.Instance.pData.standings[faction] > 0)
        {
            Greet();
        } else
        {
            ChatBot.Instance.DisplayMessage(messages["NotAllies"], true);
        }
    }
}
