using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour {

    Quaternion og_Rot;

    float pitch, yaw;
    public float pitch_speed, yaw_speed;

    private void Start()
    {
        og_Rot = transform.localRotation;
    }

    private void LateUpdate()
    {
        pitch = (Input.GetAxis("CameraPitch") * pitch_speed) + 32;
        yaw = Input.GetAxis("CameraYaw") * yaw_speed;

        //transform.Rotate(pitch, yaw, 0);

        transform.localEulerAngles = new Vector3(Mathf.Clamp(pitch, 25f, 50f),
    Mathf.Clamp(yaw, -8f, 8f), 0);

    }
}
