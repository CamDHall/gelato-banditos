using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StationAction : ScriptableObject {
    public abstract void Act(StationController controller);
}
