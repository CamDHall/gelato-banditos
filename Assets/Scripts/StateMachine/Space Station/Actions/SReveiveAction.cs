using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Station AI/Actions/Receive")]
public class SReveiveAction : StationAction {

    public override void Act(StationController controller)
    {
        DataManager.Save(CharacterManager.Instance.pData);
        SceneManager.LoadScene("SpaceStation");
    }
}
