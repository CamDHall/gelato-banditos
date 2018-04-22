using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationRailGun : MonoBehaviour {

    public float health;
    public GameObject rail;
    public float firingRate;
    public float rotationSpeed;
    float timer;
    Quaternion target;
    SpaceStation sp;

    // Temp
    float hitTimer = 0, hitTimerAmount = 0.5f;
    bool flashing = false;
    Material mat;
    Color og;

	void Start () {
        mat = GetComponent<MeshRenderer>().material;
        og = mat.color;

        timer = Time.timeSinceLevelLoad;
        sp = transform.parent.GetComponentInChildren<SpaceStation>();
	}

    private void Update()
    {
        target = Quaternion.LookRotation(PlayerMovement.player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, rotationSpeed);

        ///
        /// Temp
        ///

        if(hitTimer > Time.timeSinceLevelLoad && !flashing)
        {
            flashing = true;
            mat.color = Color.black;
        }

        if(flashing && Time.timeSinceLevelLoad > hitTimer)
        {
            flashing = false;
            mat.color = og;
        }

        ///
        /// Temp
        ///
    }

    void FixedUpdate () {

        if (timer < Time.timeSinceLevelLoad)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit[] hits = Physics.SphereCastAll(ray, 5, 5000);

            bool clearShot = false;
            
            foreach(RaycastHit hit in hits)
            {
                if (hit.transform == transform) continue;
                if (hit.transform.tag == "StationWeapons")
                {
                    break;
                }

                if (hit.transform.tag == "Player") clearShot = true;
            }

            if (clearShot)
            {
                GameObject temp = Instantiate(rail, transform.parent);
                temp.transform.position = transform.position + transform.forward * 50;
                temp.transform.rotation = transform.rotation;

                timer = Time.timeSinceLevelLoad + firingRate;
            }
        }
	}

    public void TakeDamage(int amount)
    {
        health -= amount;

        if(health <= 0)
        {
            sp.weapons.Remove(gameObject);
            Destroy(gameObject);
        }

        hitTimer = Time.timeSinceLevelLoad + hitTimerAmount;
    }
}
