using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour {

    public float speed, rotationSpeed;
    public float fireCooldown;
    public int attentionDist, range;
    public LayerMask playerMask;
    [HideInInspector]
    public Rigidbody rb;
    public EnemyState currentSate;
    public bool reachedPlayer;

    bool aiActive = false;

    public Vector3 destination;
    public float timer = 0, reachedTimer = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start () {
		
	}
	
	void Update () {
        if(Vector3.Distance(transform.position, PlayerMovement.player.transform.position) < attentionDist)
        {
            aiActive = true;
        } else
        {
            aiActive = false;
        }

		if(aiActive)
        {
            currentSate.UpdateState(this);
        }
	}
}
