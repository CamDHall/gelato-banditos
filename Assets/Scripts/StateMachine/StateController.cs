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
    public EnemyState currentSate;
    public bool reachedPlayer;

    bool aiActive = false;

    float test = 5;

    [HideInInspector]
    public float timer = 0, reachedTimer = 0, turning = 0;

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
            currentSate.FixedActions(this);
        }
    }
}
