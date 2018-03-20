using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public static PlayerMovement player;
    public Transform gelatoContainer;
    public float health, shield;
    public float thrustSpeed;

    public float shieldTimerAmount;
    float shieldTimer;

    public float deadZone;

    public float pitch_speed, yaw_speed, roll_speed;

    public bool rotating = false;

    float pitch, yaw, roll;
    Vector3 vel;

    public float accelRate, deccelTime, maxSpeed;
    public float acceleration = 0;

    // Dashing
    [SerializeField] float remainingDash = 0;
    public float dashAmount;

    public Rigidbody rb;

    public BoxCollider[] colliders;
    [HideInInspector] public float startHealth, startShield;
    public bool rolling = true;

    float deflectionTimer = 0;
    Quaternion deflectedAngle;
    Vector3 deflectedPos;

    Vector3 addedPos;
    Quaternion addedRot;

	void Awake () {
        accelRate *= 10;

        player = this;
        rb = GetComponent<Rigidbody>();

        colliders = GetComponents<BoxCollider>();

        accelRate = thrustSpeed * Time.deltaTime;
        startHealth = health;
        startShield = shield;
    }

    private void Update()
    {
        // Switch between rolling and camera
        if (Input.GetButton("CameraSwitch"))
        {
            if (rolling)
                rolling = false;
            else
                rolling = true;
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.game_over)
        {
            if (shield <= 0 && shieldTimer < Time.timeSinceLevelLoad) shield = startShield;

            if(deflectionTimer > Time.timeSinceLevelLoad)
            {
                Vector3 tempPos = Vector3.Slerp(rb.position, rb.position + deflectedPos, 0.25f);
                rb.MovePosition(tempPos);
                addedRot = Quaternion.Slerp(rb.rotation, deflectedAngle, Time.deltaTime);
            }
            // Deadzone
            Vector2 stickInput = new Vector2(Input.GetAxis("Pitch"), Input.GetAxis("Yaw"));
            if (stickInput.magnitude <= deadZone)
            {
                stickInput = Vector2.zero;
            }
            else
            {
                stickInput = stickInput.normalized * ((stickInput.magnitude - deadZone) / (1 - deadZone));
            }

            if (Input.GetAxis("Thrust") != 0)
            {
                if (acceleration < maxSpeed)
                {
                    acceleration += (accelRate * Time.deltaTime);
                }
            }
            else
            {
                if (acceleration > 0)
                {
                    if (acceleration < 0.01f)
                    {
                        acceleration = 0;
                    }
                    else
                    {
                        acceleration = Mathf.Lerp(acceleration, 0, deccelTime * Time.deltaTime);
                    }
                }
            }

            pitch = stickInput.x * pitch_speed;
            yaw = stickInput.y * yaw_speed;

            if (rolling)
            {
                roll = Input.GetAxis("Roll") * roll_speed;
            } else
            {
                roll = 0;
            }
            Quaternion rot = Quaternion.Euler(pitch, yaw, roll);
            rb.MoveRotation(rb.rotation * rot);

            if (pitch == 0 && yaw == 0 && roll == 0)
            {
                rotating = true;
            }
            else
            {
                rotating = false;
            }

            // Dash
            if (Input.GetButtonDown("DashRight"))
            {
                if (remainingDash == 0)
                {
                    remainingDash = dashAmount;
                }
            }
            if (Input.GetButtonDown("DashLeft"))
            {
                if (remainingDash == 0)
                {
                    remainingDash = -dashAmount;
                }
            }

            if (remainingDash == 0)
            {
                rb.MovePosition(rb.position + (transform.forward * acceleration));
            }
            else
            {
                remainingDash = Mathf.Lerp(remainingDash, 0, 0.2f);
                Vector3 vDash = rb.position + (transform.right * remainingDash);

                rb.MovePosition(vDash);

                if (Mathf.Abs(remainingDash) < 0.1f)
                {
                    remainingDash = 0;
                }
            }
        }
    }

    public void TakeDamge(int amount)
    {
        if (shield > 0)
        {
            Debug.Log(amount);
            shield -= amount;
        }
        else
        {
            shieldTimer = Time.timeSinceLevelLoad + shieldTimer;
            health -= amount;
            if (health <= 0)
            {
                GameManager.Instance.Death();
            }
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "Astro")
        {
            TakeDamge(1);
            Vector3 dir = coll.contacts[0].point - rb.position;
            dir = -dir.normalized;
            DeflectPlayer(dir);
        }
    }

    void DeflectPlayer(Vector3 reflectPos)
    {
        float scale = acceleration;

        Quaternion newAngle = Quaternion.Euler(rb.rotation.x * scale,
            rb.rotation.y * scale, rb.rotation.y * scale);

        deflectedPos = reflectPos * scale * 5;
        deflectionTimer = Time.timeSinceLevelLoad + 1;
        deflectedAngle = newAngle;
    }
}
