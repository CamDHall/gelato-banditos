using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 6.0F;
    public float rotationSpeed;
    public float gravity = 20.0F;

    public float deadZone;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private Vector3 currentRot;
    private Camera mainCam;
    private Vector3 camEulers;

    // Deadzone
    Vector2 leftStick, rightStick, bodyRotation, cameraRotation;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCam = GetComponentInChildren<Camera>();

        bodyRotation = Vector2.zero;
        cameraRotation = Vector2.zero;
    }

    private void Update()
    {
        rightStick = new Vector2(Input.GetAxis("RVertical"), Input.GetAxis("RHorizontal"));
        //rightStick = new Vector3(0, Input.GetAxis("RHorizontal"), 0);

        // Deadzone
        if (rightStick.magnitude <= deadZone)
        {
            rightStick = Vector2.zero;
        }
        else
        {
            rightStick = rightStick.normalized * ((rightStick.magnitude - deadZone) / (1 - deadZone));
        }
    }

    void FixedUpdate()
    {
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

        }

        camEulers = mainCam.transform.localRotation.eulerAngles;

        if ((camEulers.x > 25 && rightStick.x > 0 && camEulers.x < 333) || ( camEulers.x > 27 && camEulers.x < 335 && rightStick.x < 0))
            rightStick.x = 0;

        bodyRotation.y = rightStick.y;
        cameraRotation.x = rightStick.x;

        transform.Rotate(bodyRotation * rotationSpeed);

        mainCam.transform.Rotate(cameraRotation);

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "SceneTrigger")
        {
            CharacterUI.Instance.LeaveScene();
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if(coll.tag == "SceneTrigger")
        {
            CharacterUI.Instance.Reset();
        }
    }
}