using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StationTransition {
    public StationDecision decision;
    public StationState trueState, falseState;

}
