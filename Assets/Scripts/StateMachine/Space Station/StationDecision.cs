using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StationDecision : ScriptableObject {

    public abstract int StandingDecision(StationController controller);
    public abstract bool Decision(StationController controller);
}
