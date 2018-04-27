using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationController : MonoBehaviour {

    public StationState currentState;
    public StationState remainState;
    [HideInInspector] public bool aiActive;
    [HideInInspector] public bool cutScene;
    [HideInInspector] public bool weaponsActive;
    [HideInInspector] public SpaceStation stationObj;
    [HideInInspector] public Affilation station_afil;

	void Start () {
        stationObj = GetComponent<SpaceStation>();
        station_afil = stationObj.spaceStation_affil;
        aiActive = false;
        cutScene = false;
	}
	
	void Update () {
        if (!aiActive) return;
        currentState.UpdateState(this);
	}

    public void TransitionToState(StationState nextState)
    {
        if(currentState != nextState)
        {
            CameraManager.Instance.Reset();
            currentState = nextState;
        }
    }
}
