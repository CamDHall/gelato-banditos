using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ShowOdinSerializedPropertiesInInspector]
public class StationWeapon : MonoBehaviour, IDamageable, IDeath {

    public float health;
    public GameObject projectile;
    public float lowFiringRate, highFiringRate;
    public float rotationSpeed;

    public int resAmount;
    public string name;
    public bool friendly;

    [Sirenix.Serialization.OdinSerialize] public Dictionary<ResourceType, int> cost;

    protected float timer;
    protected Quaternion targetRot;
    protected SpaceStation sp;
    protected GameObject target;
    // Temp
    protected float hitTimer = 0, hitTimerAmount = 0.5f;
    protected bool flashing = false;
    protected Material mat;
    protected Color og;
    protected bool isEnabled;

    protected void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        og = mat.color;

        timer = Time.timeSinceLevelLoad;
        if (!friendly)
        {
            sp = transform.parent.GetComponentInChildren<SpaceStation>();
            target = PlayerMovement.player.gameObject;
        }
    }

    protected void Update()
    {
        if (!isEnabled) return;
        if (friendly && target == null)
        {
            bool targetFound = false;
            Collider[] colls = Physics.OverlapSphere(transform.position, 1000);

            foreach(Collider col in colls)
            {
                if(col.transform.tag == "StationWeapons" && col.transform != transform)
                {
                    targetFound = true;
                    target = col.gameObject;
                    break;
                }
            }

            if (!targetFound)
            {
                foreach (Collider col in colls)
                {
                    if (col.transform.tag == "Bandit" || col.transform.tag == "Fighter")
                    {
                        targetFound = true;
                        target = col.gameObject;
                        break;
                    }
                }
            }

            if(!targetFound && colls.Length > 0)
            {
                target = colls[0].gameObject;
            }
        }

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

        Debug.Log(hitTimerAmount);
        hitTimer = Time.timeSinceLevelLoad + hitTimerAmount;
    }

    public void Disable()
    {
        isEnabled = false;
        GetComponent<Collider>().enabled = false;
    }

    public void Enable()
    {
        isEnabled = true;
        GetComponent<Collider>().enabled = true;
    }

    public void Death()
    {
        ResourceType res = (ResourceType)Random.Range(0, System.Enum.GetValues(typeof(ResourceType)).Length);

        if (CharacterManager.Instance.pData.resources.ContainsKey(res))
        {
            CharacterManager.Instance.pData.resources[res] += resAmount;
        }
        else
        {
            CharacterManager.Instance.pData.resources.Add(res, resAmount);
        }

        sp.weapons.Remove(gameObject);
        transform.parent.gameObject.GetComponentInChildren<SpaceStation>().TakeDamage(20);
        Destroy(gameObject);
    }
}
