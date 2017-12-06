using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/PatrolAction")]
public class PatrolAction : Action
{
    public override void Act(StateController controller)
    {
        /*if(controller.paddingDist < Vector3.Distance(PlayerMovement.player.transform.position, controller.transform.position))
        {
            PickDestination(controller);
        }*/

        PathFinding(controller);

        float dist = Vector3.Distance(PlayerMovement.player.transform.position, controller.transform.position);

        if(dist > controller.maxRange)
        {
            controller.rb.MovePosition(controller.rb.position + (controller.transform.forward * controller.speed));
        } else
        {
            Vector3 newPos = controller.transform.forward * -PlayerMovement.player.acceleration;
            controller.rb.MovePosition(controller.rb.position + newPos + Strafe(controller));
        }
    }

    Vector3 Strafe(StateController controller)
    {
        if (controller.strafePos == Vector3.zero)
        {
            controller.strafePos = controller.transform.right * (1 + (controller.strafeStrength * Time.deltaTime));
            controller.lastDirection = controller.strafePos;
        } else {
            if (controller.strafeTimer < Time.time)
            {
                controller.strafePos = -controller.lastDirection;
                controller.lastDirection = controller.strafePos;
                controller.strafeTimer = Time.time + 1.25f;
            } else
            {
                controller.strafePos *= (1 + (controller.strafeStrength * Time.deltaTime));
            }
        }

        return controller.strafePos;
    }

    void PathFinding(StateController controller)
    {
        Transform transform = controller.transform;
        Vector3 Pos = controller.rb.position;
        Vector3 offset = Vector3.zero;
        Quaternion newRotation = Quaternion.identity;

        RaycastHit hit;
        Vector3 up = Pos + (controller.transform.up * controller.rayCastOffset);
        Vector3 down = Pos - (controller.transform.up * controller.rayCastOffset);
        Vector3 left = Pos - (controller.transform.right * controller.rayCastOffset);
        Vector3 right = Pos + (controller.transform.right * controller.rayCastOffset);

        if (Physics.Raycast(left, transform.forward, out hit, controller.detectionDist) ||
            Physics.Raycast(right, transform.forward, out hit, controller.detectionDist) ||
            Physics.Raycast(up, transform.forward, out hit, controller.detectionDist) ||
            Physics.Raycast(down, transform.forward, out hit, controller.detectionDist))
        {
             newRotation = Quaternion.LookRotation(Vector3.Cross(controller.transform.position,
                hit.transform.position));

        } else
        {
            newRotation = Quaternion.LookRotation(PlayerMovement.player.transform.position - controller.transform.position);
        }

        controller.rb.rotation = Quaternion.Slerp(controller.rb.rotation, newRotation, Time.deltaTime * controller.rotationSpeed);
    }
}
