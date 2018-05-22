using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public static PlayerMovement player;
    public Transform gelatoContainer;
    public float startHealth, startShield;
    public float thrustSpeed;

    public float shieldTimerAmount;
    public float rechargeRate;
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
    [HideInInspector] public float health, shield;
    public float dashAmount;

    public Rigidbody rb;

    public BoxCollider[] colliders;
    public bool rolling = true;

    float deflectionTimer = 0;
    Quaternion deflectedAngle;
    Vector3 deflectedPos;

    Vector3 addedPos;

    // Update saves
    Vector2 stickInput;
    float thrust;
    bool dashRight = false, dashLeft = false, reversing = false;

	void Awake () {
        accelRate *= 10;

        player = this;
        rb = GetComponent<Rigidbody>();

        colliders = GetComponents<BoxCollider>();

        accelRate = thrustSpeed * Time.deltaTime;
        health = startHealth;
        shield = startShield;
    }

    private void Update()
    {
        // Inputs
        stickInput = new Vector2(Input.GetAxis("Pitch"), Input.GetAxis("Yaw"));
        thrust = Input.GetAxis("Thrust");

        if(Input.GetAxis("Thrust") > 0 && Input.GetButtonDown("DashLeft"))
        {
            reversing = true;
        }

        if (reversing && Input.GetAxis("Thrust") <= 0 || Input.GetButtonUp("DashLeft")) reversing = false;

        // Switch between rolling and camera
        if (Input.GetButton("CameraSwitch"))
        {
            if (rolling)
                rolling = false;
            else
                rolling = true;
        }

        //Shield recharge
        if(shieldTimer < Time.timeSinceLevelLoad && shield < startShield)
        {
            float newAmount = shield + (Time.deltaTime * rechargeRate);

            if(newAmount < startShield)
            {
                shield = newAmount;
            } else
            {
                shield = startShield;
            }
        }

        dashLeft = Input.GetButtonDown("DashLeft");
        dashRight = Input.GetButtonDown("DashRight");
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
            }
            // Deadzone
            if (stickInput.magnitude <= deadZone)
            {
                stickInput = Vector2.zero;
            }
            else
            {
                stickInput = stickInput.normalized * ((stickInput.magnitude - deadZone) / (1 - deadZone));
            }

            if (thrust != 0)
            {
                if (acceleration < maxSpeed)
                {
                    if (!reversing)
                    {
                        acceleration += (accelRate * Time.deltaTime);
                    } else
                    {
                        acceleration -= ((accelRate / 2) * Time.deltaTime);
                    }
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
            if (dashRight && !reversing)
            {
                if (remainingDash == 0)
                {
                    remainingDash = dashAmount;
                }
            }
            if (dashLeft && reversing)
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

    public void TakeDamge(float amount)
    {
        shieldTimer = Time.timeSinceLevelLoad + shieldTimerAmount;

        if (shield > 0)
        {
            shield -= amount;
        }
        else
        {
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

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Projectile" || coll.gameObject.tag == "Laser")
        {
            Collider currentCol = coll.GetComponent<Collider>();
            if (currentCol == colliders[1])
            {
                GameManager.Instance.Indicator("Left");
            }
            else if (currentCol == colliders[2])
            {
                GameManager.Instance.Indicator("Right");
            }
            else if (currentCol == colliders[3])
            {
                GameManager.Instance.Indicator("Front");
            }
            else if (currentCol == colliders[4])
            {
                GameManager.Instance.Indicator("Back");
            }
            else if (currentCol == colliders[5])
            {
                GameManager.Instance.Indicator("Top");
            }
            else
            {
                GameManager.Instance.Indicator("Bottom");
            }
        }
    }
}
