using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Shoot")]
public class ShootAction : Action
{
    public override void Act(StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller)
    {
        if(controller.fireCooldown < Time.time)
        {
            GameObject prefab = Resources.Load("Rocket") as GameObject;
            GameObject rocket = Instantiate(prefab, controller.transform.position, controller.transform.localRotation);

            controller.fireCooldown = Time.time + 5;
        }
    }
}
