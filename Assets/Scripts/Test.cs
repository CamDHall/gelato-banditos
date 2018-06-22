using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {


    public float deadZone;
    private CharacterController controller;
    private Vector3 currentRot;

    // Deadzone
    Vector2 leftStick, rightStick;
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        rightStick = new Vector2(Input.GetAxis("RHorizontal"), Input.GetAxis("RVertical"));

        // Deadzone
        if (rightStick.magnitude <= deadZone)
        {
            rightStick = Vector2.zero;
        }
        else
        {
            rightStick = rightStick.normalized * ((rightStick.magnitude - deadZone) / (1 - deadZone));
        }

        if ((transform.rotation.x >= 0.3f && rightStick.x > 0) || (transform.rotation.x <= -0.3f && rightStick.x < 0)) rightStick.x = 0;

        transform.Rotate(rightStick);
	}
}
