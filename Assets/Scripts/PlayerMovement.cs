using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public static PlayerMovement player;
    public float thrustSpeed;

    public float pitch_speed, yaw_speed, roll_speed;

    public bool rotating = false;

    public float pitch, yaw, roll;
    Vector3 vel;

    public float accelRate, deccelTime, maxSpeed;
    public float acceleration = 0;

    Vector3 centerPos;

    ParticleSystem ps;

    Rigidbody rb;

	void Awake () {
        player = this;
        rb = GetComponent<Rigidbody>();
        ps = GetComponentInChildren<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if(Input.GetButton("Thrust"))
        {
            if (acceleration < maxSpeed)
            {
                acceleration += (accelRate * Time.deltaTime);
                /*if (ps.isStopped)
                {
                    ps.Play();
                    ps.Simulate(1f, false, false);
                }*/
            }
        } else
        {
            if(acceleration > 0)
            {
                if(acceleration < 0.01f)
                {
                    acceleration = 0;
                    /*if(ps.isPlaying)
                    {
                        ps.Stop();
                    }*/
                } else
                {
                    acceleration = Mathf.Lerp(acceleration, 0, deccelTime * Time.deltaTime);
                }
            }
        }

        pitch = Input.GetAxis("Pitch") * pitch_speed;
        yaw = Input.GetAxis("Yaw") * yaw_speed;
        roll = Input.GetAxis("Roll") * roll_speed;

        transform.Rotate(pitch, yaw, roll);
        
        if(pitch == 0 && yaw == 0 && roll == 0)
        {
            rotating = true;
        } else
        {
            rotating = false;
        }

        rb.MovePosition(rb.position + (transform.forward * acceleration));
    }
}
