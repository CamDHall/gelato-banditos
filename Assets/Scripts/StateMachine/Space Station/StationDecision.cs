using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StationDecision : ScriptableObject {

    public abstract bool Decide(StationController controller);
}
