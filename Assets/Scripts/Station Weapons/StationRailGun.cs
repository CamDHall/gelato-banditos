using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationRailGun : StationWeapon {

    public float moveSpeed;
    float difAngle;
    int side;
    float dist;
    Vector3 modPos, heightMod;

    float posTimer;

    bool collided = false;
    float collidedTimer;
    Vector3 dir;

    private void Start()
    {
        base.Start();
        posTimer = Time.timeSinceLevelLoad;
        heightMod = transform.up * moveSpeed * Time.deltaTime;
    }

    private void Update()
    {
        base.Update();
        difAngle = Vector3.Angle(transform.forward, PlayerMovement.player.transform.forward);
        side = Utilts.AngleDir(transform.forward, PlayerMovement.player.transform.forward, transform.up);

        Vector3 oldPos = transform.position;

        if (difAngle > 142)
        {
            if(side == -1)
            {
                modPos = transform.right * moveSpeed * Time.deltaTime;
            } else
            {
                modPos = -transform.right * moveSpeed * Time.deltaTime;
            }
        } else if(difAngle > 60)
        {
            dist = Vector3.Distance(transform.position, PlayerMovement.player.transform.position);
            if (dist <= 500)
            {
                if (side == -1)
                {
                    modPos = transform.right * 10 * moveSpeed * Time.deltaTime;
                }
                else
                {
                    modPos = transform.right * -10 * moveSpeed * Time.deltaTime;
                }
            }
        } else
        {
            modPos = transform.forward * moveSpeed * Time.deltaTime;
            heightMod = Vector3.zero;
        }

        if(posTimer < Time.timeSinceLevelLoad)
        {
            posTimer = Time.timeSinceLevelLoad + 5;
            heightMod *= -1;
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, 75);

        foreach(Collider col in hits)
        {
            if (col.tag == "SpaceStation")
            {
                collided = true;
                dir = col.ClosestPoint(transform.position);
                collidedTimer = Time.timeSinceLevelLoad + 3;
                break;
            }
        }

        if (!collided)
        {
            transform.position += (modPos + heightMod);
        }

        if(collidedTimer > Time.timeSinceLevelLoad)
        {
            transform.position -= dir.normalized * (moveSpeed * 2) * Time.deltaTime;
        } else
        {
            collided = false;
        }

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
