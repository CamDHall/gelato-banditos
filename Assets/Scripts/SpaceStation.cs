using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Affilation { ChihuahuaFederation, Juarez }
public class SpaceStation : MonoBehaviour, IDamageable, IDeath {

    public Affilation spaceStation_affil;
    public float health;
    public List<GameObject> weapons;
    public int resValue;

    StationController sc;

	void Start () {
        sc = GetComponent<StationController>();
	}
	
	void Update () {
		
	}

    public void TakeDamage(int amount)
    {
        health -= amount;

        if(health <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        ResourceType res = (ResourceType)Random.Range(0, System.Enum.GetValues(typeof(ResourceType)).Length);

        if (PlayerInventory.Instance.resources.ContainsKey(res))
        {
            PlayerInventory.Instance.resources[res] += resValue;
        }
        else
        {
            PlayerInventory.Instance.resources.Add(res, resValue);
        }

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
            if(sc.aiActive)
            {
                sc.cutScene = false;
                sc.aiActive = false;
            }
        }
    }
}
