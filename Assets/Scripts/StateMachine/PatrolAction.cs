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
        Quaternion targetRotation = Quaternion.LookRotation(PlayerMovement.player.transform.position - controller.transform.position);

        controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, targetRotation, Time.deltaTime * controller.rotationSpeed);

        if(controller.timer < Time.time)
        {
            controller.destination = PlayerMovement.player.transform.position;
            controller.timer = Time.time + 10;
        }

        if (controller.reachedTimer < Time.time)
        {
            RaycastHit hit;

            if (Physics.Raycast(controller.transform.position, controller.transform.forward, out hit, 15))
            {
                controller.rb.MovePosition(controller.transform.position + (controller.transform.right * (Time.deltaTime * 10)));
            }
            else
            {
                float dist = Vector3.Distance(controller.destination, controller.transform.position);
                float playerDist = Vector3.Distance(PlayerMovement.player.transform.position, controller.transform.position);

                if (dist > 100 && playerDist > 80)
                {
                    controller.rb.MovePosition(controller.transform.position + (controller.transform.forward * controller.speed));
                } else
                {
                    controller.reachedTimer = Time.time + 10;
                }
            }
        }
    }


}
