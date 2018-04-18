using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour {

    public GameObject gelato;
    public float speed, rotationSpeed, rotationAvoid, paddingDist;
    public float strafeStrength;
    public float fireCooldown;
    public int attentionDist, maxRange, minRange;
    public float rayCastOffset, detectionDist;
    public List<State> states;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Vector3 strafePos, destination;
    [HideInInspector] List<State> currentStates = new List<State>();

    public float strafeTimer = 0, travelTimer = 0;
    public Quaternion newRotation;
    public float timer;
    public GameObject target;
    public bool isFriend = false;

    public SphereCollider sc;

    bool aiActive = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        speed *= Time.deltaTime;
        timer = fireCooldown;

        sc = GetComponent<SphereCollider>();
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
                foreach (State state in states)
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
            foreach (State state in states)
            {
                state.UpdateFixedState(this);
            }
        }
    }

    public void Die()
    {
        if(Random.Range(0, 10) > 3)
        {
            Instantiate(gelato, transform.position, Quaternion.identity);
            GameManager.Instance.score += 5;
        }

        Destroy(gameObject, Time.deltaTime * 5);
    }
}
