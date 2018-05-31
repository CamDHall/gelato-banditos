using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour {

    public float maxSpeed;
    public float range;
    LayerMask lm;
    Vector3 desiredVel, steering;
    Vector3 velocity, lastVelocity, futurePos;
    float T = 0;

	void Start () {
        lastVelocity = transform.position;
    }

    void Update () {
        if (Vector3.Distance(PlayerMovement.player.transform.position, transform.position) > range)
        {
            velocity = (velocity - lastVelocity) * Time.deltaTime;
            T = Vector3.Distance(transform.position, PlayerMovement.player.transform.position) / maxSpeed;
            futurePos = PlayerMovement.player.transform.position + (Camera.main.transform.forward * (PlayerMovement.player.acceleration * 3));
            desiredVel = (futurePos - transform.position).normalized * maxSpeed;
            steering = desiredVel - velocity;

            velocity = velocity + steering;
            transform.position += velocity;
            lastVelocity = transform.position;

            transform.rotation = Quaternion.LookRotation(velocity);
        } else
        {
            transform.LookAt(PlayerMovement.player.transform);
        }
    }
}
