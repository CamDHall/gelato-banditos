using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Station AI/Decisions/Standing")]
public class SStandingDecision : StationDecision {

    public override int StandingDecision(StationController controller)
    {
        int val = PlayerInventory.Instance.pData.standings[controller.station_afil];
        if (val == 0) return 0;
        else if (val < 0) return -1;

        return 1;
    }

    public override bool Decision(StationController controller)
    {
        throw new System.NotImplementedException();
    }
}
