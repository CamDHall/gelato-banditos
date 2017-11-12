using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public static PlayerMovement player;
    public float thrustSpeed;

    public float pitch_speed, yaw_speed, roll_speed;

    float pitch, yaw, roll;
    Vector3 vel;

    Rigidbody rb;

	void Awake () {
        player = this;
        rb = GetComponent<Rigidbody>();
	}
	
	void Update () {
        
	}

    private void FixedUpdate()
    {
        pitch = Input.GetAxis("Pitch") * pitch_speed;
        yaw = Input.GetAxis("Yaw") * yaw_speed;
        roll = Input.GetAxis("Roll") * roll_speed;

        transform.Rotate(pitch, yaw, roll);
        if(Input.GetButton("Thrust"))
        {
            rb.MovePosition(transform.position + transform.forward);
        };
        /*
        if (Input.GetButton("Thrust"))
        {
            vel = (transform.position + Vector3.forward) * (thrustSpeed * Time.deltaTime);
            Debug.Log(vel);
            transform.Translate(vel);
            //rb.MovePosition(rb.position + Vector3.forward);
        }*/
    }
}
