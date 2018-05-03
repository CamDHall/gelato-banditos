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
        if (controller.flockTimer < Time.timeSinceLevelLoad)
        {
            controller.flockTimer = Time.timeSinceLevelLoad + controller.flockRate;
            SpawnFlock(controller);
        }
    }

    void SpawnFlock(StationController controller)
    {
        float dist = Vector3.Distance(PlayerMovement.player.transform.position, controller.transform.localPosition);
        Vector3 Pos = controller.transform.position +
            (controller.transform.forward * (controller.GetComponent<Collider>().bounds.size.x / 1.5f));

        GameObject temp = Instantiate(controller.flockParent, Pos, Quaternion.identity);
        temp.transform.LookAt(Camera.main.transform);
    }
}
