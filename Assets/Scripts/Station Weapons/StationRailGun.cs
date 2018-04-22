using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationRailGun : StationWeapon {

    private void Update()
    {
        base.Update();
        target = Quaternion.LookRotation(PlayerMovement.player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, rotationSpeed);
    }

    void FixedUpdate () {

        if (timer < Time.timeSinceLevelLoad)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit[] hits = Physics.SphereCastAll(ray, 5, 5000);

            bool clearShot = false;
            
            foreach(RaycastHit hit in hits)
            {
                if (hit.transform == transform) continue;
                if (hit.transform.tag == "StationWeapons")
                {
                    break;
                }

                if (hit.transform.tag == "Player") clearShot = true;
            }

            if (clearShot)
            {
                GameObject temp = Instantiate(projectile, transform.parent);
                temp.transform.position = transform.position + transform.forward * 50;
                temp.transform.rotation = transform.rotation;

                timer = Time.timeSinceLevelLoad + firingRate;
            }
        }
	}
}
