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
        float dist = Vector3.Distance(PlayerMovement.player.transform.position, controller.transform.position);


        Debug.Log(Quaternion.FromToRotation(Vector3.up, PlayerMovement.player.transform.position - controller.transform.position).eulerAngles.z);

        PathFinding(controller);
        if (dist > controller.paddingDist)
        {
            Move(controller);
        } else if(Quaternion.FromToRotation(Vector3.up, PlayerMovement.player.transform.position - controller.transform.position).eulerAngles.z <= 90 ||
            Quaternion.FromToRotation(Vector3.up, PlayerMovement.player.transform.position - controller.transform.position).eulerAngles.z >= 270)
        {
            Strafe(controller);
        }
    }

    void Move(StateController controller)
    {
        controller.rb.MovePosition(controller.transform.position + (controller.transform.forward * controller.speed));
    }

    void Turn(StateController controller)
    {
        Vector3 Pos = PlayerMovement.player.transform.position - controller.transform.position;
        Quaternion newRotation = Quaternion.LookRotation(Pos);
        Quaternion rotation = Quaternion.Slerp(controller.transform.rotation, newRotation, Time.deltaTime * controller.rotationSpeed);
        controller.rb.MoveRotation(rotation);
    }

    void Turn(StateController controller, float modifier)
    {
        Vector3 Pos = PlayerMovement.player.transform.position - controller.transform.position;
        Quaternion newRotation = Quaternion.LookRotation(Pos);
        Quaternion rotation = Quaternion.Slerp(controller.transform.rotation, newRotation, Time.deltaTime * (controller.rotationSpeed * modifier));
        controller.rb.MoveRotation(rotation);
    }

    void PathFinding(StateController controller)
    {
        RaycastHit hit;
        Transform cPos = controller.transform;

        Debug.DrawRay(controller.rb.position + (cPos.right * controller.rayCastOffset), controller.transform.forward * controller.detectionDist, Color.white);

        if (Physics.Raycast(controller.rb.position + (cPos.right * controller.rayCastOffset), cPos.forward, out hit, controller.detectionDist) ||
            Physics.Raycast(controller.rb.position - (cPos.right * controller.rayCastOffset), cPos.forward, out hit, controller.detectionDist) ||
            Physics.Raycast(controller.rb.position + (cPos.up * controller.rayCastOffset), cPos.forward, out hit, controller.detectionDist) ||
            Physics.Raycast(controller.rb.position - (cPos.up * controller.rayCastOffset), cPos.forward, out hit, controller.detectionDist))
        {
            Vector3 target = hit.transform.position - controller.transform.position;
            Quaternion rotation = Quaternion.LookRotation(target);
            Vector3 euler = rotation.eulerAngles;
            euler.y -= 180;

            rotation = Quaternion.Euler(euler);
            controller.rb.rotation = Quaternion.Slerp(controller.rb.rotation, rotation, Time.deltaTime);
            controller.turning = Time.time + 1.2f;
        }
        else if(controller.turning < Time.time)
        {
            Turn(controller, 2);
        }
    }

    void Strafe(StateController controller)
    {
        controller.rb.MovePosition(controller.rb.position + (controller.transform.right * controller.speed));
    }
}
