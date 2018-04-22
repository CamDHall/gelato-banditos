using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationWeapon : MonoBehaviour {

    public float health;
    public GameObject projectile;
    public float firingRate;
    public float rotationSpeed;

    protected float timer;
    protected Quaternion target;
    protected SpaceStation sp;

    // Temp
    protected float hitTimer = 0, hitTimerAmount = 0.5f;
    protected bool flashing = false;
    protected Material mat;
    protected Color og;

    private void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        og = mat.color;

        timer = Time.timeSinceLevelLoad;

        sp = transform.parent.GetComponentInChildren<SpaceStation>();
    }

    public void Update()
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
            sp.weapons.Remove(gameObject);
            Destroy(gameObject);
        }

        hitTimer = Time.timeSinceLevelLoad + hitTimerAmount;
    }
}
