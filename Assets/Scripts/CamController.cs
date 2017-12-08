using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour {

    float pitch = 0, yaw = 0;
    public float pitch_speed, yaw_speed;
    public float deadZone;

    Quaternion newRotation;

    private void LateUpdate()
    {
        // Deadzone
        Vector2 stickInput = new Vector2(Input.GetAxis("CameraPitch"), Input.GetAxis("CameraYaw"));
        if (stickInput.magnitude <= deadZone) stickInput = Vector2.zero;


        if (stickInput.magnitude <= deadZone)
        {
            stickInput = Vector2.zero;
        }
        else
        {
            stickInput = stickInput.normalized * ((stickInput.magnitude - deadZone) / (1 - deadZone));
        }

        if (stickInput.x != 0)
        {
            pitch += (stickInput.x * pitch_speed);
        } else
        {
            pitch = 0;
        }
        if (stickInput.y != 0)
        {
            yaw += stickInput.y * yaw_speed;
        } else
        {
            yaw = 0;
        }
        pitch = Mathf.Clamp(pitch, -15, 10);
        yaw = Mathf.Clamp(yaw, -18, 18);

        newRotation = Quaternion.Euler(pitch, yaw, 0);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, newRotation, 0.1f);

        //Vector3 newRotation = new Vector3(Mathf.Clamp(pitch, -50f, 50f),
            //Mathf.Clamp(yaw, -30f, 30f), 0);

        //transform.localEulerAngles = Vector3.Slerp(transform.localEulerAngles, newRotation, 0.1f);
        
        /*
        transform.localEulerAngles = new Vector3(Mathf.Clamp(pitch, -50f, 50f),
    Mathf.Clamp(yaw, -20f, 20f), 0);*/

    }
}
