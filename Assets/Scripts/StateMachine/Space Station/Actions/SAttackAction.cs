using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Station AI/Actions/Attack")]
public class SAttackAction : StationAction {

    public override void Act(StationController controller)
    {
        if(!controller.weaponsActive)
        {
            foreach(GameObject weapon in controller.stationObj.weapons)
            {
                weapon.SetActive(true);
            }
        }
    }
}
