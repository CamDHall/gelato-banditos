using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour {

    public float speed, rotationSpeed, rotationAvoid, paddingDist;
    public float fireCooldown;
    public int attentionDist, range;
    public float rayCastOffset, detectionDist;
    public LayerMask playerMask;
    [HideInInspector]
    public Rigidbody rb;
    public State currentSate;
    public bool reachedPlayer;
    public Vector3 destination, lastHit;

    bool aiActive = false;

    float test = 5;

    [HideInInspector]
    public float patrolResetTimer = 0, reachedTimer = 0, turning = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
	
	void Update () {
        if (!GameManager.Instance.game_over)
        {
            if (Vector3.Distance(transform.position, PlayerMovement.player.transform.position) < attentionDist)
            {
                aiActive = true;
            }
            else
            {
                aiActive = false;
            }

            if (aiActive)
            {
                currentSate.UpdateState(this);
            }
        }
	}

    private void FixedUpdate()
    {
        if(!GameManager.Instance.game_over && aiActive)
        {
            currentSate.UpdateFixedState(this);
        }
    }
}
