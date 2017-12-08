using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Shoot")]
public class ShootAction : Action
{
    public override void Act(StateController controller)
    {
        if (Vector3.Distance(controller.transform.position, PlayerMovement.player.transform.position) < controller.attentionDist)
        {
            Attack(controller);
        }
    }

    private void Attack(StateController controller)
    {
        if(controller.fireCooldown < Time.time)
        {
            GameObject prefab = Resources.Load("Rocket") as GameObject;
            Instantiate(prefab, controller.transform.position + (controller.transform.forward * 25), controller.transform.localRotation, GameManager.Instance.bulletContainer.transform);

            controller.fireCooldown = Time.time + 1;
        }
    }
}
