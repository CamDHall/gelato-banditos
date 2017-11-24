using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour {

    float pitch = 0, yaw = 0;
    public float pitch_speed, yaw_speed;

    Quaternion newRotation;

    private void LateUpdate()
    {
        if (Input.GetAxis("CameraPitch") != 0)
        {
            pitch += (Input.GetAxis("CameraPitch") * pitch_speed);
        } else
        {
            pitch = 0;
        }
        if (Input.GetAxis("CameraYaw") != 0)
        {
            yaw += Input.GetAxis("CameraYaw") * yaw_speed;
        } else
        {
            yaw = 0;
        }

        newRotation = Quaternion.Euler(Mathf.Clamp(pitch, -15, 10), Mathf.Clamp(yaw, -18, 18), 0);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, newRotation, 0.1f);

        //Vector3 newRotation = new Vector3(Mathf.Clamp(pitch, -50f, 50f),
            //Mathf.Clamp(yaw, -30f, 30f), 0);

        //transform.localEulerAngles = Vector3.Slerp(transform.localEulerAngles, newRotation, 0.1f);
        
        /*
        transform.localEulerAngles = new Vector3(Mathf.Clamp(pitch, -50f, 50f),
    Mathf.Clamp(yaw, -20f, 20f), 0);*/

    }
}
