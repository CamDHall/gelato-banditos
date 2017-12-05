using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/PatrolAction")]
public class PatrolAction : Action
{
    public override void Act(StateController controller)
    {
        if(controller.patrolResetTimer < Time.time)
        {
            PickDestination(controller);
        } else
        {
            PathFinding(controller);
        }
    }

    void PickDestination(StateController controller)
    {
        Vector3 centerPoint = (PlayerMovement.player.transform.position - controller.transform.position) * 0.5f;
        Vector3 newDest = centerPoint + (Random.insideUnitSphere * 5);

        controller.destination = newDest;
        Debug.Log(centerPoint);
        controller.patrolResetTimer = Time.time + 5;
    }

    void PathFinding(StateController controller)
    {
        Transform transform = controller.transform;
        Vector3 Pos = controller.rb.position;
        Vector3 offset = Vector3.zero;

        RaycastHit hit;
        Vector3 up = Pos + (controller.transform.up * controller.rayCastOffset);
        Vector3 down = Pos - (controller.transform.up * controller.rayCastOffset);
        Vector3 left = Pos - (controller.transform.right * controller.rayCastOffset);
        Vector3 right = Pos + (controller.transform.right * controller.rayCastOffset);

        Debug.DrawRay(right, transform.forward * controller.detectionDist, Color.white);

        if(Physics.Raycast(left, transform.forward, out hit, controller.detectionDist))
        {
            offset += transform.right;
            controller.lastHit = transform.right;
        } else if(Physics.Raycast(right, transform.forward, out hit, controller.detectionDist))
        {
            offset -= transform.right;
            controller.lastHit = -transform.right;
        } else if(Physics.Raycast(up, transform.forward, out hit, controller.detectionDist))
        {
            offset -= transform.up;
            controller.lastHit = transform.up;
        } else if(Physics.Raycast(down, transform.forward, out hit, controller.detectionDist))
        {
            offset += transform.up;
            controller.lastHit = -transform.up;
        }

        if(offset != Vector3.zero)
        {
            controller.transform.Rotate(offset * controller.rotationAvoid);
            //controller.rb.MoveRotation(Quaternion.FromToRotation(controller.rb.position, offset * 5f * Time.deltaTime));
        }
        else if(controller.lastHit == Vector3.zero ||
            !Physics.Raycast(controller.lastHit * controller.rayCastOffset, controller.lastHit, out hit, controller.detectionDist))
        {
            controller.lastHit = Vector3.zero;
            Quaternion newRotation = Quaternion.LookRotation(controller.destination - Pos);
            controller.rb.rotation = Quaternion.Slerp(controller.rb.rotation, newRotation, Time.deltaTime * controller.rotationSpeed);
        }

        controller.rb.MovePosition(controller.rb.position + (transform.forward * controller.speed));
    }
}
