using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationController : MonoBehaviour {

    public StationState currentState;
    public StationState remainState;
    [HideInInspector] public bool aiActive;
    [HideInInspector] public bool cutScene;

    [HideInInspector] public Affilation station_afil;

	void Start () {
        station_afil = GetComponent<SpaceStation>().spaceStation_affil;
        aiActive = false;
        cutScene = false;
	}
	
	void Update () {
        if (!aiActive) return;
        currentState.UpdateState(this);
	}

    public void TransitionToState(StationState nextState)
    {
        if(remainState != nextState)
        {
            currentState = nextState;
        }
    }
}
