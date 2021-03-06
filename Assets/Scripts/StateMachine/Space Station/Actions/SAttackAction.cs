﻿using System.Collections;
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
                weapon.GetComponent<StationWeapon>().Enable();
            }

            controller.flockTimer = Time.timeSinceLevelLoad + 3;
            controller.fighterSpawnTimer = Time.timeSinceLevelLoad + 10;
            CameraManager.Instance.Reset();
            controller.weaponsActive = true;
        }

        if (controller.flockTimer < Time.timeSinceLevelLoad)
        {
            controller.flockTimer = Time.timeSinceLevelLoad + controller.flockRate;
            SpawnFlock(controller);
        }

        if(controller.fighterSpawnTimer < Time.timeSinceLevelLoad)
        {
            SpawnFighters(controller);
            controller.fighterSpawnTimer = Time.timeSinceLevelLoad + controller.fighterSpawnRate;
        }
    }

    void SpawnFlock(StationController controller)
    {
        Vector3 Pos = controller.transform.position +
            (controller.transform.forward * (controller.GetComponent<Collider>().bounds.size.x / 1.5f));

        GameObject temp = Instantiate(controller.flockParent, Pos, Quaternion.identity);
        temp.transform.LookAt(Camera.main.transform);
    }

    void SpawnFighters(StationController controller)
    {
        Vector3 Pos = Random.insideUnitSphere * 450 + controller.transform.parent.transform.position;
        GameObject temp = Instantiate(controller.fighterPrefab, Pos, Quaternion.identity);
        temp.transform.LookAt(PlayerMovement.player.transform);
    }
}
