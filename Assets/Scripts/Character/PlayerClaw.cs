using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClaw : MonoBehaviour {


    /// <summary>
    /// Automated tethering should be an upgrade
    /// 
    /// </summary>
    public float range, speed;
    bool throwClaw, isPullingIn;
    GameObject tetheredObj;
    StationWeapon sp;

    Vector3 dir;

	void Start () {
		
	}
	
	void Update () {
        throwClaw = Input.GetButtonUp("Y") & !isPullingIn;

        if(throwClaw)
        {
            if(isPullingIn)
            {
                GameManager.Instance.EnablePlayer();
                isPullingIn = false;
                sp.isTethered = false;
            }
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, range))
            {
                if (hit.transform.tag == "StationWeapons")
                {
                    tetheredObj = hit.transform.gameObject;
                    sp = hit.transform.GetComponent<StationWeapon>();
                    GameManager.Instance.DisablePlayer();

                    isPullingIn = true;
                    sp.isTethered = true;
                }
            }
        }

        if(isPullingIn && tetheredObj == null)
        {
            isPullingIn = false;
            GameManager.Instance.EnablePlayer();
        } else if(isPullingIn)
        {
            dir = (transform.position - tetheredObj.transform.position).normalized;
            tetheredObj.transform.position += (dir * speed);

            if (Vector3.Distance(transform.parent.transform.position, tetheredObj.transform.position) < 200)
            {
                Capture();
                isPullingIn = false;
                GameManager.Instance.EnablePlayer();
                tetheredObj.SetActive(false);
            }
        }
	}

    void Capture()
    {
        if (CharacterManager.Instance.pData.weapons == null) CharacterManager.Instance.pData.weapons = new Dictionary<string, List<GameObject>>();
        if(CharacterManager.Instance.pData.weapons.ContainsKey(tetheredObj.transform.name))
        {
            CharacterManager.Instance.pData.weapons[tetheredObj.transform.name].Add(tetheredObj);
        } else
        {
            List<GameObject> newWeapons = new List<GameObject>();
            newWeapons.Add(tetheredObj);
            CharacterManager.Instance.pData.weapons.Add(tetheredObj.transform.name, newWeapons);
        }
    }
}
