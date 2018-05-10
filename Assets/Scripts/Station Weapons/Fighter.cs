using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour, IDamageable {

    public float health;
    public LineRenderer prefab_laser;
    public float attackRate;


    float attackTimer;

    // Use this for initialization
    void Start () {
        attackTimer = Time.timeSinceLevelLoad + attackRate;
    }
	
	// Update is called once per frame
	void Update () {
        if (attackTimer < Time.timeSinceLevelLoad)
        {
            Utilts.FireLaser(transform, prefab_laser);
            attackTimer = Time.timeSinceLevelLoad + attackRate;
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if(health == 0)
        {
            Destroy(gameObject);
        }
    }
}
