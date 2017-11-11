using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public static PlayerMovement player;
    public float thrustSpeed;

    float pitch, yaw, roll;
    Quaternion _rotation;
    Vector3 vel;

    Rigidbody rb;

	void Start () {
        player = this;
        rb = GetComponent<Rigidbody>();
        _rotation = Quaternion.identity;
	}
	
	void Update () {
        
	}

    private void FixedUpdate()
    {
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");
        roll = Input.GetAxis("Roll");

        _rotation.eulerAngles = new Vector3(pitch, yaw, roll);
        rb.rotation *= _rotation;

        Debug.Log(pitch);

        if (Input.GetButton("Thrust"))
        {
            vel = (transform.position + Vector3.forward) * (thrustSpeed * Time.deltaTime);
            Debug.Log(vel);
            transform.Translate(vel);
            //rb.MovePosition(rb.position + Vector3.forward);
        }
    }
}
