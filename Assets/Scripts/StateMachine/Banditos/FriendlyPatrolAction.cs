using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="PluggableAI/Actions/FriendlyPatrolAction")]
public class FriendlyPatrolAction : Action {

    public override void Act(StateController controller)
    {
        if (controller.isFriend)
        {
            FindTarget(controller);
            if (controller.target != null)
            {
                Aim(controller);
            }
        }
    }

    void FindTarget(StateController controller)
    {
        Collider[] colls = Physics.OverlapSphere(controller.transform.position, 1000);
        List<GameObject> banditos = new List<GameObject>();

        foreach(Collider col in colls)
        {
            if(col.gameObject.tag == "Bandito" && col.gameObject != controller.gameObject)
            {
                banditos.Add(col.gameObject);
            }
        }

        float closestDist = 50000;
        GameObject closest = null;

        foreach(GameObject bandit in banditos)
        {
            float dist = Vector3.Distance(controller.transform.position, bandit.transform.position);
            if(dist < closestDist)
            {
                closest = bandit;
                closestDist = dist;
            }
        }

        controller.target = closest;
    }

    void Aim(StateController controller)
    {
        controller.transform.LookAt(controller.target.transform.position);
    }
}
