using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour {
    public static CamController Instance;

    float pitch = 0, yaw = 0;
    public float pitch_speed, yaw_speed;
    public float deadZone;
    public float shakeLength, shakeInt;
    float shakeTimer = 0;

    Vector3 _hitPos;
    Quaternion newRotation;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if(shakeTimer > Time.timeSinceLevelLoad)
        {
            //transform.LookAt((Time.deltaTime * Time.deltaTime * Time.deltaTime) * (transform.position - _hitPos));
        }
    }

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

    }

    public void ShakeCamera(Vector3 hitPos)
    {
        if (shakeTimer < Time.timeSinceLevelLoad)
        {
            _hitPos = hitPos;
            shakeTimer = Time.timeSinceLevelLoad + (shakeLength * Time.deltaTime);
        }
    }
}
