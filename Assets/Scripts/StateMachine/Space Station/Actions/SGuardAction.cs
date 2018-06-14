﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Station AI/Actions/Guard")]
public class SGuardAction : StationAction
{
    public override void Act(StationController controller)
    {
        CameraManager.Instance.SpaceStationGuardScene(controller.gameObject);
    }
}
