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
                float dist = Vector3.Distance(hit.transform.position, controller.transform.position);

                if (dist > 10)
                {
                    if (!controller.reachedPlayer)
                    {
                        MoveForward(controller);
                    }
                }
                else
                {
                    controller.reachedPlayer = true;
                }

                if(controller.reachedPlayer && dist > 400)
                {
                    controller.reachedPlayer = false;
                }
            } else
            {
                MoveAround(controller, hit.transform.gameObject);
            }
        } else
        {
            MoveForward(controller);
        }
    }

    void MoveForward(StateController controller)
    {
        controller.rb.MovePosition(controller.transform.position + (controller.transform.forward * controller.speed));
    }

    void MoveAround(StateController controller, GameObject obstacle)
    {
        controller.rb.MovePosition(controller.transform.position + (controller.transform.right * (Time.deltaTime * 10)));
    }
}
