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


        if(Physics.Raycast(controller.transform.position, controller.transform.forward, out hit, 10))
        {
            MoveAround(controller, hit.transform.gameObject);
        } else
        {
            float dist = Vector3.Distance(PlayerMovement.player.front.position, controller.transform.position);

            if (dist > 20)
            {
                if (!controller.reachedPlayer)
                {
                    MoveForward(controller);
                }
            }
            else if (dist < 30)
            {
                Circle(controller);
            }
            else
            {
                controller.reachedPlayer = true;
            }

            if (controller.reachedPlayer && dist > 400)
            {
                controller.reachedPlayer = false;
            }
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

    void Circle(StateController controller)
    {
        int direction = Random.Range(-1, 1);
        Vector3 dest = (controller.transform.forward + (controller.transform.right * direction)) * (controller.speed / 100);

        controller.rb.MovePosition(controller.transform.position + dest);
    }
}
