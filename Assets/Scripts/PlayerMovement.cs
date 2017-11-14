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

    Vector3 centerPos;

    Rigidbody rb;

	void Awake () {
        player = this;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
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

        if (Input.GetButton("Thrust"))
        {
            rb.MovePosition(rb.position + transform.forward);
        }
    }
}
