using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour {

    public Rigidbody rb;
    public EnemyState currentSate;
    bool aiActive = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start () {
		
	}
	
	void Update () {
        if(Vector3.Distance(transform.position, PlayerMovement.player.transform.position) < 100)
        {
            aiActive = true;
        } else
        {
            aiActive = false;
        }

		if(!aiActive)
        {
            return;
        }

        currentSate.UpdateState(this);
	}
}
