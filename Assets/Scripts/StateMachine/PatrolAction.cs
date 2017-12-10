using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/PatrolAction")]
public class PatrolAction : Action
{
    public override void Act(StateController controller)
    {
        if (!PathFinding(controller))
        {

            float dist = Vector3.Distance(PlayerMovement.player.transform.position, controller.transform.position);

            if (dist > controller.maxRange)
            {
                controller.rb.MovePosition(controller.rb.position + (controller.transform.forward * controller.speed));
            }
            else
            {
                Vector3 newPos = Vector3.zero;
                if (dist < controller.minRange)
                {
                    if (PlayerMovement.player.acceleration <= 0)
                    {
                        newPos = controller.transform.forward * -1;
                    }
                    else
                    {
                        newPos = controller.transform.forward * -PlayerMovement.player.acceleration;
                    }
                }
                controller.rb.MovePosition(controller.rb.position + newPos + Strafe(controller));
            }
        }
    }

    Vector3 Strafe(StateController controller)
    {
        if (controller.strafePos == Vector3.zero)
        {
            controller.strafePos = controller.transform.right * (1 + (controller.strafeStrength * Time.deltaTime));
        } else {
            if (controller.strafeTimer < Time.timeSinceLevelLoad)
            {
                controller.strafePos = new Vector3(Random.Range(-1f, 1f) * Time.deltaTime
                    , Random.Range(-1f, 1f) * Time.deltaTime, 0);
                controller.strafeTimer = Time.timeSinceLevelLoad + 2f;
            } else
            {
                controller.strafePos *= (1 + (controller.strafeStrength * Time.deltaTime));
            }
        }

        return controller.strafePos;
    }

    bool PathFinding(StateController controller)
    {
        bool avoiding = false;
        Transform transform = controller.transform;
        Vector3 Pos = controller.rb.position;
        Vector3 offset = Vector3.zero;
        Quaternion newRotation = Quaternion.identity;

        Collider[] colls = Physics.OverlapSphere(controller.transform.position, controller.sc.radius + controller.rayCastOffset);

        if (colls.Length > 1)
        {
            newRotation = Quaternion.LookRotation(Vector3.Cross(controller.transform.position,
                colls[1].transform.position));
            avoiding = true;
        }
        else
        {
            newRotation = Quaternion.LookRotation(PlayerMovement.player.transform.position - controller.transform.position);
            avoiding = false;
        }

        controller.rb.rotation = Quaternion.Slerp(controller.rb.rotation, newRotation, Time.deltaTime * controller.rotationSpeed);
        if (avoiding)
        {
            controller.rb.MovePosition(controller.rb.position + (controller.transform.forward * controller.speed));
        }
        return avoiding;
    }
}
