using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour {

    public float speed;
    public float fireCooldown;
    public int attentionDist;

    [HideInInspector]
    public Rigidbody rb;
    public EnemyState currentSate;
    public bool reachedPlayer;

    bool aiActive = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start () {
		
	}
	
	void Update () {
        if(Vector3.Distance(transform.position, PlayerMovement.player.front.position) < attentionDist)
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
