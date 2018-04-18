using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Station AI/Decisions/Standing")]
public class SStandingDecision : StationDecision {

    public override bool Decide(StationController controller)
    {
        if (CharacterManager.Instance.standings[controller.station_afil] == 0) return true;

        return false;
    }
}
