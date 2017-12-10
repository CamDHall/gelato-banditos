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
        if(controller.timer < Time.timeSinceLevelLoad)
        {
            GameObject prefab = Resources.Load("Rocket") as GameObject;
            Instantiate(prefab, controller.transform.position + (controller.transform.forward * 30), controller.transform.localRotation, GameManager.Instance.bulletContainer.transform);

            controller.timer = Time.timeSinceLevelLoad + (Random.Range(controller.fireCooldown, controller.fireCooldown + 2));
        }
    }
}
