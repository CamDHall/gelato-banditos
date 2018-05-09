﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour {

    public float maxSpeed;
    public float range;
    public LineRenderer prefab_laser;
    public float attackRate;
    LayerMask lm;

    float attackTimer;
    Vector3 desiredVel, steering;
    Vector3 velocity, lastVelocity;

	void Start () {
        lastVelocity = transform.position;
        attackTimer = Time.timeSinceLevelLoad + attackRate;
    }

    void Update () {
        if (Vector3.Distance(PlayerMovement.player.transform.position, transform.position) > range)
        {
            velocity = (velocity - lastVelocity) * Time.deltaTime;
            desiredVel = (PlayerMovement.player.transform.position - transform.position).normalized * maxSpeed;
            steering = desiredVel - velocity;

            velocity = velocity + steering;
            transform.position += velocity;
            lastVelocity = transform.position;

            transform.rotation = Quaternion.LookRotation(velocity);
        } else
        {
            transform.LookAt(PlayerMovement.player.transform);
        }

        if(attackTimer < Time.timeSinceLevelLoad)
        {
            Utilts.FireLaser(transform, prefab_laser);
            attackTimer = Time.timeSinceLevelLoad + attackRate;
        }
    }
}