using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour {

    public float speed, rotationSpeed, rotationAvoid, paddingDist;
    public float strafeStrength;
    public float fireCooldown;
    public int attentionDist, maxRange;
    public float rayCastOffset, detectionDist;

    [HideInInspector]
    public Rigidbody rb;
    public State[] currentSates;
    public Vector3 strafePos, lastDirection;
    public float strafeTimer = -1;

    bool aiActive = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        speed *= Time.deltaTime;
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
                foreach (State state in currentSates)
                {
                    state.UpdateState(this);
                }
            }
        }
	}

    private void FixedUpdate()
    {
        if(!GameManager.Instance.game_over && aiActive)
        {
            foreach (State state in currentSates)
            {
                state.UpdateFixedState(this);
            }
        }
    }
}
