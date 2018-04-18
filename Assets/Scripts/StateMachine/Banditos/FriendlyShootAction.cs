using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/FriendlyShootAction")]
public class FriendlyShootAction : Action {
    public override void Act(StateController controller)
    {
        if(controller.target != null && controller.timer < Time.timeSinceLevelLoad)
        {
            Attack(controller);
        }
    }

    void Attack(StateController controller)
    {
        GameObject prefab = Resources.Load("Rocket") as GameObject;
        Instantiate(prefab, controller.transform.position + (controller.transform.forward * 30), controller.transform.localRotation, GameManager.Instance.bulletContainer.transform);

        controller.timer = Time.timeSinceLevelLoad + (Random.Range(controller.fireCooldown, controller.fireCooldown + 2));
    }
}
