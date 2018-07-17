using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStation : MonoBehaviour, IDamageable, IDeath {

    public Faction spaceStation_affil;
    public float health;
    public List<GameObject> weapons;
    public int resValue;

    public StationController sc;
    List<ChatMessage> deathMessage;

    bool leftArea = false;
    float leftTimer;

	void Start () {
        sc = GetComponent<StationController>();

        deathMessage = new List<ChatMessage>();
        deathMessage.Add(new ChatMessage(ChatBot.Instance.data.GOOD_NEWS_COLOR, "Congratulations, you destoryed a Space Station belonging to the "
            + spaceStation_affil.ToString() + " Faction.\n\n"));
        deathMessage.Add(new ChatMessage(ChatBot.Instance.data.NEWS_COLOR, "Remeber: You can often resolve conflcits without violence."));
	}
	
	void Update () {
		/*if(sc.aiActive && leftArea && leftTimer < Time.timeSinceLevelLoad)
        {
            sc.cutScene = false;
            sc.aiActive = false;
        }*/ 

        /// COME BACK TO
	}

    public void TakeDamage(int amount)
    {
        CharacterManager.Instance.pData.standings[spaceStation_affil] -= amount * 15;
        health -= amount;
        sc.currentState.CheckTransitions(sc);
        sc.aiActive = true;
        if (health <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        ResourceType res = (ResourceType)Random.Range(0, System.Enum.GetValues(typeof(ResourceType)).Length);

        if (CharacterManager.Instance.pData.resources.ContainsKey(res))
        {
            CharacterManager.Instance.pData.resources[res] += resValue;
        }
        else
        {
            CharacterManager.Instance.pData.resources.Add(res, resValue);
        }

        ChatBot.Instance.DisplayMessage(deathMessage, true, 12);
        Destroy(gameObject.transform.parent.gameObject);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            if (!sc.aiActive)
            {
                GameManager.Instance.nearestStation = this;
                sc.aiActive = true;
            }
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if(coll.tag == "Player")
        {
            leftArea = true;
            sc.aiActive = false;
            leftTimer = Time.timeSinceLevelLoad + 10;
        }
    }
}
