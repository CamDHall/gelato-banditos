using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
public class PatrolAction : Action
{
    public override void Act(StateController controller)
    {
        Patrol(controller);
    }

    private void Patrol(StateController controller)
    {
        controller.transform.LookAt(PlayerMovement.player.transform);

        RaycastHit hit;


        if(Physics.Raycast(controller.transform.position, controller.transform.forward, out hit))
        {
            if(hit.transform.gameObject.tag == "Player")
            {
                Move(controller);
            } else
            {
                Debug.Log("BLOCKED");
            }
        } else
        {
            Move(controller);
        }
    }

    void Move(StateController controller)
    {
        controller.rb.MovePosition(controller.transform.position + (controller.transform.forward * Time.deltaTime));
    }
}
