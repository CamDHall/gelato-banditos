using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Station AI/Actions/Receive")]
public class SReveiveAction : StationAction {

    public override void Act(StationController controller)
    {
        if (!CinematicUI.Instance.storePanel.gameObject.activeSelf)
        {
            if (controller.cutScene)
            {
                CinematicUI.Instance.SetupStore(controller);
            } else
            {
                CameraManager.Instance.SpaceStationGuardScene(controller.gameObject);
            }
        }
    }
}
