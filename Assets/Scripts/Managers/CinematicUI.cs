using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicUI : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {

	}

    public void Attack()
    {
        Debug.Log("ATTACK STATION");
    }

    public void Leave()
    {
        float col_size = GameManager.Instance.nearestStation.GetComponent<SphereCollider>().radius;
        Collider[] colls = Physics.OverlapSphere(PlayerMovement.player.transform.position, col_size);
        PlayerMovement.player.transform.position += (PlayerMovement.player.transform.forward * -(col_size + 1500));

        foreach(Collider col in colls)
        {
            if(col.gameObject.name.Contains("SpaceStation"))
            {
                PlayerMovement.player.transform.LookAt(col.gameObject.transform);
                break;
            }
        }

        CameraManager.Instance.Reset();
    }

    public void GiveGelato()
    {
        Debug.Log("GIVE GELATO");
    }
}
