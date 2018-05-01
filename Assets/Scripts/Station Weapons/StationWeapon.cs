using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationWeapon : MonoBehaviour, IDamageable, IDeath {

    public float health;
    public GameObject projectile;
    public float firingRate;
    public float rotationSpeed;

    public int resAmount;

    protected float timer;
    protected Quaternion target;
    protected SpaceStation sp;

    // Temp
    protected float hitTimer = 0, hitTimerAmount = 0.5f;
    protected bool flashing = false;
    protected Material mat;
    protected Color og;

    protected void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        og = mat.color;

        timer = Time.timeSinceLevelLoad;

        sp = transform.parent.GetComponentInChildren<SpaceStation>();
    }

    protected void Update()
    {
        ///
        /// Temp
        ///
        if (hitTimer > Time.timeSinceLevelLoad && !flashing)
        {
            flashing = true;
            mat.color = Color.black;
        }

        if (flashing && Time.timeSinceLevelLoad > hitTimer)
        {
            flashing = false;
            mat.color = og;
        }

        ///
        /// Temp
        ///
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Death();
        }

        hitTimer = Time.timeSinceLevelLoad + hitTimerAmount;
    }

    public void Death()
    {
        ResourceType res = (ResourceType)Random.Range(0, System.Enum.GetValues(typeof(ResourceType)).Length);

        if (PlayerInventory.Instance.resources.ContainsKey(res))
        {
            PlayerInventory.Instance.resources[res] += resAmount;
        }
        else
        {
            PlayerInventory.Instance.resources.Add(res, resAmount);
        }

        sp.weapons.Remove(gameObject);
        transform.parent.gameObject.GetComponentInChildren<SpaceStation>().TakeDamage(20);
        Destroy(gameObject);
    }
}
