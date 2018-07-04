using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Station AI/Actions/Receive")]
public class SReveiveAction : StationAction {

    public override void Act(StationController controller)
    {
        if (!CinematicUI.Instance.storePanel.gameObject.activeSelf && !controller.hasSaved)
        {
            controller.hasSaved = true;
            DataManager.Save(PlayerInventory.Instance.playerData);
            SceneManager.LoadScene("SpaceStation");
            //CinematicUI.Instance.SetupStore(controller);
            //CameraManager.Instance.SpaceStationGuardScene(controller.gameObject);
        }
    }
}
