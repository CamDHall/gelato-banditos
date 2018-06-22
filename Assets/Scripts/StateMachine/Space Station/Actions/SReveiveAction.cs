using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Station AI/Actions/Receive")]
public class SReveiveAction : StationAction {

    public override void Act(StationController controller)
    {
        if (!CinematicUI.Instance.storePanel.gameObject.activeSelf)
        {
            //Serializer.Save<PlayerInventory>("playerInventory.txt", PlayerInventory.Instance);
            CinematicUI.Instance.SetupStore(controller);
            CameraManager.Instance.SpaceStationGuardScene(controller.gameObject);
        }
    }
}
